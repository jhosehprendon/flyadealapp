using System.Collections.Generic;

namespace Newskies.WebApi.Configuration
{

    public class InterceptorsSettings
    {
        public string[] Assemblies { get; set; }
        public Interceptor[] Interceptors { get; set; }
        public InterceptorParameters[] Parameters { get; set; }
    }

    public class Interceptor
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public PropertyInterceptor[] Validation { get; set; }
        public RequestInterceptor[] Request { get; set; }
        public ResponseInterceptor[] Response { get; set; }
    }

    public class ResponseInterceptor
    {
        public string Type { get; set; }
        public string[] Interceptors { get; set; }
    }

    public class RequestInterceptor
    {
        public string Type { get; set; }
        public string ParameterName { get; set; }
        public string[] Interceptors { get; set; }
    }

    public class PropertyInterceptor
    {
        public string Type { get; set; }
        public string Property { get; set; }
        public string[] Interceptors { get; set; }
    }

    public class InterceptorParameters
    {
        public string InterceptorType { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }

    //public class InterceptorAttributesSettings
    //{
    //    public string FileName { get; set; }
    //    public ActionFilterInterceptorSettings ActionFilterInterceptorSettings { get; set; }
    //}

    //public class ActionFilterInterceptorSettings
    //{
    //    public string Namespace { get; set; }
    //    public string ActionFilterInterceptorFullClassName { get; set; }
    //    public string ValidationInterceptorFullClassName { get; set; }
    //    public ActionFilterInterceptor[] ActionFilterInterceptors { get; set; }
    //}

    //public class ActionFilterInterceptor
    //{
    //    public string Controller { get; set; }
    //    public string Action { get; set; }
    //    public string[] ValidationMethods { get; set; }
    //    public Dictionary<string, string> Settings { get; set; }
    //}


}
