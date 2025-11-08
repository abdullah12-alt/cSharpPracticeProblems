using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.DTO;
 public record AuthenticationResponse(Guid Guid, string Email, string? PersonName, string? Gender, string? Token, bool Success);











//Conceptual Breakdown
//Namespace (eCommerce.Core.DTO): The namespace defines the logical grouping of your code.Here, the Core.DTO namespace represents that the file belongs to the core layer of your eCommerce application and contains DTOs.

//record Keyword: This defines a record type in C#. A record is a reference type that provides built-in functionality for value equality, meaning two records with the same data are considered equal, unlike classes which use reference equality.

//Benefits of Records: Records are immutable by default. You can define a set of properties that don’t change, making them perfect for DTOs since they only carry data. You also get automatic ToString(), Equals(), and GetHashCode() implementations.

//Properties:

//Guid UserID: Stores the user's unique identifier.

//string? Email: The user’s email. The? denotes it is a nullable string.

//string? PersonName: The user’s name, also nullable.

//string? Gender: Represents the gender of the user, nullable string.

//string? Token: Stores the authentication token.

//bool Success: A flag to indicate whether the authentication was successful.

//Parameterless Constructor: The empty constructor is required for some deserialization frameworks or APIs that need to create objects without parameters.

//The this (default, default, ...) call assigns default values(e.g., null for reference types, Guid.Empty for Guid, false for bool) to each parameter when no arguments are provided.

