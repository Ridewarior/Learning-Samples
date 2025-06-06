﻿namespace Movies.Api.Auth;

public static class AuthConstants
{
    public const string AdminUserPolicy = "Admin";
    public const string AdminUserClaim = "admin";
    public const string TrustedMemberPolicy = "Trusted";
    public const string TrustedMemberClaim = "trusted_member";
    
    public const string ApiKeyHeaderName = "x-api-key";
    
}