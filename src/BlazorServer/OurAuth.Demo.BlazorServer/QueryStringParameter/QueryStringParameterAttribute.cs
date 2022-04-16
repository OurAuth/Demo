// Copyright (c) 2020 Gérald Barré
// See: https://www.meziantou.net/bind-parameters-from-the-query-string-in-blazor.htm

namespace OurAuth.Demo.BlazorServer.Shared;

/// <summary>
/// A query string <see cref="ParameterAttribute"/> for Blazor Page. <seealso cref="QueryStringParameterExtensions"/>
/// </summary>
/// <remarks>Nullable value types not supported yet.</remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class QueryStringParameterAttribute : Attribute
{
    public QueryStringParameterAttribute()
    {
    }

    public QueryStringParameterAttribute(string name)
    {
        Name = name;
    }

    /// <summary>Name of the query string parameter. It uses the property name by default.</summary>
    public string Name { get; }
}
