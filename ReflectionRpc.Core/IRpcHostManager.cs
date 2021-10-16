﻿namespace ReflectionRpc.Core
{
    public interface IRpcHostManager
    {
        RegisteredRpcHost RegisterHost(object rpcTarget, string tag = null);
        
        List<RegisteredRpcHost> GetHosts();
        
        RegisteredRpcHost GetHost(Guid guid);
        
        RegisteredRpcHost GetHost(string tag);
    }
}
