using System;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using DynamicRouting.security;
using Microsoft.AspNetCore.Http;
using DynamicRouting;

namespace DynamicRoutingTests
{
        public class APIKeyValidatorTest
        {
            private readonly ILoggerFactory loggerFactory = (ILoggerFactory)new LoggerFactory();

            private readonly string[] DEFAULT_ALL_HOSTS = { "*" };

            private readonly string[] DEFAULT_API_KEY = { "1234", "124" };

            private readonly string[] CUSTOMIZED_HOST_LIST = { "localhost", "google.com" };

            private readonly string[] CUSTOMZIED_KEY_LIST_1234 = { "1234" };

            private readonly string[] CUSTOMZIED_KEY_LIST_124 = { "124" };

            [Fact]
            public async Task APIKeyValidator_KeyNotPresentHostAllowed_ShouldReturn401APIKeyNotPresent()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = DEFAULT_ALL_HOSTS;
                httpContext.Items[RestConst.HEADER_API_KEY] = DEFAULT_API_KEY;
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.Unauthorized, httpContext.Response.StatusCode);

            }


            [Fact]
            public async Task APIKeyValidator_KeyIncorrectHostAllowed_ShouldReturn401IncorrectKey()
            {

                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = DEFAULT_ALL_HOSTS;
                httpContext.Items[RestConst.HEADER_API_KEY] = DEFAULT_API_KEY;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "122";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.Unauthorized, httpContext.Response.StatusCode);

            }

            [Fact]
            public async Task APIKeyValidator_ValidKeyHostAllowed_ShouldReturn200()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = DEFAULT_ALL_HOSTS;
                httpContext.Items[RestConst.HEADER_API_KEY] = DEFAULT_API_KEY;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "1234";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.OK, httpContext.Response.StatusCode);

            }

            [Fact]
            public async Task APIKeyValidator_HostNotAllowed_ShouldReturn401()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = CUSTOMIZED_HOST_LIST;
                httpContext.Items[RestConst.HEADER_API_KEY] = CUSTOMZIED_KEY_LIST_1234;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "124";
                httpContext.Request.Headers["Host"] = "127.0.0.1";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.Unauthorized, httpContext.Response.StatusCode);
            }
            [Fact]
            public async Task APIKeyValidator_HostNotComplete_ShouldReturn401()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = CUSTOMIZED_HOST_LIST;
                httpContext.Items[RestConst.HEADER_API_KEY] = CUSTOMZIED_KEY_LIST_1234;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "124";
                httpContext.Request.Headers["Host"] = "gle.com";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.Unauthorized, httpContext.Response.StatusCode);
            }
            [Fact]
            public async Task APIKeyValidator_ApiKeyOnList_ShouldReturn200()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = DEFAULT_ALL_HOSTS;
                httpContext.Items[RestConst.HEADER_API_KEY] = DEFAULT_API_KEY;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "124";
                httpContext.Request.Headers["Host"] = "gle.com";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.OK, httpContext.Response.StatusCode);
            }
            [Fact]
            public async Task APIKeyValidator_HostAllowedApiKeyMatch_ShouldReturn200()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = CUSTOMIZED_HOST_LIST;
                httpContext.Items[RestConst.HEADER_API_KEY] = DEFAULT_API_KEY;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "124";
                httpContext.Request.Headers["Host"] = "google.com";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.OK, httpContext.Response.StatusCode);
            }

            [Fact]
            public async Task APIKeyValidator_HostAllowedApiKeyNotMatch_ShouldReturn401()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = CUSTOMIZED_HOST_LIST;
                httpContext.Items[RestConst.HEADER_API_KEY] = DEFAULT_API_KEY;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "1245";
                httpContext.Request.Headers["Host"] = "google.com";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.Unauthorized, httpContext.Response.StatusCode);
            }
            [Fact]
            public async Task APIKeyValidator_NoRoutingConfigured_ShouldReturn404()
            {
                var requestDelegate = new RequestDelegate(
                        (innerContext) => Task.FromResult(0));
                //var httpContextMock = new Mock<HttpContext>();
                var httpContext = new DefaultHttpContext();
                httpContext.Items[RestConst.ALLOWED_HOSTS] = null;
                httpContext.Items[RestConst.HEADER_API_KEY] = null;
                httpContext.Request.Headers[RestConst.HEADER_API_KEY] = "1245";
                httpContext.Request.Headers["Host"] = "google.com";
                var validator = new APIKeyValidator(requestDelegate, loggerFactory);
                await validator.InvokeAsync(httpContext);
                Assert.Equal((double)HttpStatusCode.NotFound, httpContext.Response.StatusCode);
            }
        }

    }


