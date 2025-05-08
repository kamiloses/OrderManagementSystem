namespace OrderManagementSystem.Middlewares;

public class SimpleMiddleware
{
    private readonly RequestDelegate _next;

    public SimpleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.BackgroundColor = ConsoleColor.Magenta;
        Console.WriteLine("ZAWSZE SIE WYKONAM");
        await _next(context);
    }
}
