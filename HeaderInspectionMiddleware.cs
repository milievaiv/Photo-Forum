namespace PhotoForum
{
    public class HeaderInspectionMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderInspectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log or inspect headers here
            var headers = context.Response.Headers;

            Console.WriteLine(context.Response);

            await _next(context);
        }
    }
}
