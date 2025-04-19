using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Authentication;

namespace Domain;

public class CircuitBreakerPolicyProvider
{
    public AsyncCircuitBreakerPolicy<HttpResponseMessage> GetPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<SocketException>()
            .Or<AuthenticationException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (result, breakDelay) =>
                {
                    Console.WriteLine($"Open circuit by {breakDelay.TotalSeconds} seconds.");
                },
                onReset: () =>
                {
                    Console.WriteLine("Closed circuit. Service available.");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Circuit in limited state.");
                });
    }
}