namespace SqlDAL.DAL
{
    internal class HttpResponseMessage
    {
        private object badRequest;

        public HttpResponseMessage(object badRequest)
        {
            this.badRequest = badRequest;
        }

        public StringContent Content { get; set; }
    }
}