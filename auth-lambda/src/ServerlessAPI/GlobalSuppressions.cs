// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S6667:Logging in a catch clause should pass the caught exception as a parameter.", Justification = "<Pendente>", Scope = "member", Target = "~M:ServerlessAPI.Program.Main(System.String[])")]
[assembly: SuppressMessage("Major Code Smell", "S112:General or reserved exceptions should never be thrown", Justification = "<Pendente>", Scope = "member", Target = "~M:ServerlessAPI.Services.CognitoService.IdentifiqueSeAsync(System.String,System.String,System.String,System.Threading.CancellationToken)~System.Threading.Tasks.Task{ServerlessAPI.Models.Response.TokenUsuarioResponse}")]
