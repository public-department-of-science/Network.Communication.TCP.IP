using System;

namespace TCP.IP.Communication.Client
{
    public partial class TcpClient
    {
        /// <summary>
        /// Establish a connection to the server.
        /// </summary>
        public void Connect()
        {
            if (IsConnected)
            {
                Logger?.Invoke($"{_header}already connected");
                return;
            }
            else
            {
                Logger?.Invoke($"{_header}initializing client");
                InitializeClient(_ssl, _pfxCertFilename, _pfxPassword);
                Logger?.Invoke($"{_header}connecting to {ServerIpPort}");
            }

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _token.Register(() =>
            {
                if (!_ssl)
                {
                    if (_sslStream == null)
                    {
                        return;
                    }
                    _sslStream.Close();
                }
                else
                {
                    if (_networkStream == null)
                    {
                        return;
                    }
                    _networkStream.Close();
                }
            });

            IAsyncResult ar = _client.BeginConnect(_serverIp, _serverPort, null, null);
            WaitHandle wh = ar.AsyncWaitHandle;

            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(_settings.ConnectTimeoutMs), false))
                {
                    _client.Close();
                    throw new TimeoutException($"Timeout connecting to {ServerIpPort}");
                }

                _client.EndConnect(ar);
                _networkStream = _client.GetStream();
                _networkStream.ReadTimeout = _settings.ReadTimeoutMs;

                if (_ssl)
                {
                    if (_settings.AcceptInvalidCertificates)
                    {
                        _sslStream = new SslStream(_networkStream, false, new RemoteCertificateValidationCallback(AcceptCertificate));
                    }
                    else
                    {
                        _sslStream = new SslStream(_networkStream, false);
                    }

                    _sslStream.ReadTimeout = _settings.ReadTimeoutMs;
                    _sslStream.AuthenticateAsClient(_serverIp, _sslCertCollection, SslProtocols.Tls12, !_settings.AcceptInvalidCertificates);

                    if (!_sslStream.IsEncrypted)
                    {
                        throw new AuthenticationException("Stream is not encrypted");
                    }
                    if (!_sslStream.IsAuthenticated)
                    {
                        throw new AuthenticationException("Stream is not authenticated");
                    }
                    if (_settings.MutuallyAuthenticate && !_sslStream.IsMutuallyAuthenticated)
                    {
                        throw new AuthenticationException("Mutual authentication failed");
                    }
                }

                if (_keepalive.EnableTcpKeepAlives) EnableKeepalives();
            }
            catch (Exception)
            {
                throw;
            }

            _isConnected = true;
            _lastActivity = DateTime.UtcNow;
            _isTimeout = false;
            _events.HandleConnected(this, new ConnectionEventArgs(ServerIpPort));
            _dataReceiver = Task.Run(() => DataReceiver(_token), _token);
            _idleServerMonitor = Task.Run(IdleServerMonitor, _token);
            _connectionMonitor = Task.Run(ConnectedMonitor, _token);
        }

        /// <summary>
        /// Establish the connection to the server with retries up to either the timeout specified or the value in Settings.ConnectTimeoutMs.
        /// </summary>
        /// <param name="timeoutMs">The amount of time in milliseconds to continue attempting connections.</param>
        public void ConnectWithRetries(int? timeoutMs = null)
        {
            if (timeoutMs != null && timeoutMs < 1) throw new ArgumentException("Timeout milliseconds must be greater than zero.");
            if (timeoutMs != null) _settings.ConnectTimeoutMs = timeoutMs.Value;

            if (IsConnected)
            {
                Logger?.Invoke($"{_header}already connected");
                return;
            }
            else
            {
                Logger?.Invoke($"{_header}initializing client");

                InitializeClient(_ssl, _pfxCertFilename, _pfxPassword);

                Logger?.Invoke($"{_header}connecting to {ServerIpPort}");
            }

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _token.Register(() =>
            {
                if (!_ssl)
                {
                    if (_sslStream == null)
                    {
                        return;
                    }
                    _sslStream.Close();
                }
                else
                {
                    if (_networkStream == null)
                    {
                        return;
                    }
                    _networkStream.Close();
                }
            });


            using (CancellationTokenSource connectTokenSource = new CancellationTokenSource())
            {
                CancellationToken connectToken = connectTokenSource.Token;

                Task cancelTask = Task.Delay(_settings.ConnectTimeoutMs, _token);
                Task connectTask = Task.Run(() =>
                {
                    int retryCount = 0;

                    while (true)
                    {
                        try
                        {
                            string msg = $"{_header}attempting connection to {_serverIp}:{_serverPort}";
                            if (retryCount > 0)
                            {
                                msg += $" ({retryCount} retries)";
                            }

                            Logger?.Invoke(msg);

                            _client.Dispose();
                            _client = new System.Net.Sockets.TcpClient();
                            _client.ConnectAsync(_serverIp, _serverPort).Wait(1000, connectToken);

                            if (_client.Connected)
                            {
                                Logger?.Invoke($"{_header}connected to {_serverIp}:{_serverPort}");
                                break;
                            }
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception e)
                        {
                            Logger?.Invoke($"{_header}failed connecting to {_serverIp}:{_serverPort}: {e.Message}");
                        }
                        finally
                        {
                            retryCount++;
                        }
                    }
                }, connectToken);

                Task.WhenAny(cancelTask, connectTask).Wait();

                if (cancelTask.IsCompleted)
                {
                    connectTokenSource.Cancel();
                    _client.Close();
                    throw new TimeoutException($"Timeout connecting to {ServerIpPort}");
                }

                try
                {
                    _networkStream = _client.GetStream();
                    _networkStream.ReadTimeout = _settings.ReadTimeoutMs;

                    if (_ssl)
                    {
                        if (_settings.AcceptInvalidCertificates)
                        {
                            _sslStream = new SslStream(_networkStream, false, new RemoteCertificateValidationCallback(AcceptCertificate));
                        }
                        else
                        {
                            _sslStream = new SslStream(_networkStream, false);
                        }

                        _sslStream.ReadTimeout = _settings.ReadTimeoutMs;
                        _sslStream.AuthenticateAsClient(_serverIp, _sslCertCollection, SslProtocols.Tls12, !_settings.AcceptInvalidCertificates);

                        if (!_sslStream.IsEncrypted)
                        {
                            throw new AuthenticationException("Stream is not encrypted");
                        }
                        if (!_sslStream.IsAuthenticated)
                        {
                            throw new AuthenticationException("Stream is not authenticated");
                        }
                        if (_settings.MutuallyAuthenticate && !_sslStream.IsMutuallyAuthenticated)
                        {
                            throw new AuthenticationException("Mutual authentication failed");
                        }
                    }

                    if (_keepalive.EnableTcpKeepAlives)
                    {
                        EnableKeepalives();
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

            _isConnected = true;
            _lastActivity = DateTime.UtcNow;
            _isTimeout = false;
            _events.HandleConnected(this, new ConnectionEventArgs(ServerIpPort));
            _dataReceiver = Task.Run(() => DataReceiver(_token), _token);
            _idleServerMonitor = Task.Run(IdleServerMonitor, _token);
            _connectionMonitor = Task.Run(ConnectedMonitor, _token);
        }
    }
}
