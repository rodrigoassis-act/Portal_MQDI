namespace ONS.PortalMQDI.Shared.Utils
{
    public static class PortalMQDIUtil
    {

        public static bool Violacao(bool valor)
        {
            return !valor ? true : false;
        }
    }
}
