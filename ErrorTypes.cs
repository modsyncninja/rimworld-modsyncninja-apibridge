namespace ModSyncNinjaApiBridge
{
    /// <summary>
    /// 1001 – Not found
    /// 1002 – Deleted mod, 
    /// 1003 – Unverified mod, 
    /// 1004 – Database error
    /// Error code range 1000-2000

    /// </summary>
    public enum ModVersionApiErrors
    {
        NoError = 0,
        NotFound = 1001,
        DeletedMod = 1002,
        UnverifiedMod = 1003,
        DatabaseError = 1004,
        UnknownError = 1005
    };


    /// <summary>
    /// 2001 – Attempt to update to an older version(for example updating mod v2 to v1 if v2 is already in DB)
    /// 2002 – Not found
    /// 2003 – Mod+Key combo invalid (wrong password)
    /// 2004 – Missing \ 0 length version code (should be handled client side as well)
    /// 2005 – Deleted mod, 2006 – Unverified mod, 2007 – Database error
    /// 2008 – Patch attributes invalid (bugs,features,translations) markup
    /// 2009 – Patch notes format invalid (unused, for future reference)
    /// Error code range 2000-3000
    /// </summary>
    public enum UpdateModApiErrors
    {
        NoError = 0,
        OldVersion = 2001,
        NotFound = 2002,
        InvalidKey = 2003,
        NoVersion = 2004,
        DeletedMod = 2005,
        UnverifiedMod = 2006,
        DatabaseError = 2007,
        InvalidAttributes = 2008,
        InvalidPatchNotes = 2009,
        UnknownError = 2010
    };
}
