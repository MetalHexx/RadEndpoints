namespace RadEndpoints
{
    public static class RadTypeExtensions 
    {
        public static bool IsAssignableToGenericType(this object obj, Type genericType)
        {
            var type = obj.GetType();
            var interfaces = type.GetInterfaces();
            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
