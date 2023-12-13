﻿namespace Spent.Commons.Attributes;

/// <summary>
///     Instead of repeatedly applying the ErrorMessageResourceType to properties featuring validation attributes like
///     [Required] or [StringLength],
///     you can streamline the process by applying this attribute to the class just once.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DtoResourceTypeAttribute(Type resourceType) : Attribute
{
    public Type ResourceType { get; } = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
}
