const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
    app.use(
        '/api',
        createProxyMiddleware({
            target: 'http://localhost:5678',
            changeOrigin: true,
        })
    );
    app.use(
        '/login/google-login',
        createProxyMiddleware({
            target: 'http://localhost:5678',
            changeOrigin: true,
        })
    );
    app.use(
        '/login/google-response',
        createProxyMiddleware({
            target: 'http://localhost:5678',
            changeOrigin: true,
        })
    );
    app.use(
        '/logout',
        createProxyMiddleware({
            target: 'http://localhost:5678',
            changeOrigin: true,
        })
    );
};
