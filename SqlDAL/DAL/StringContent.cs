using System;
using System.Net.Http;

namespace SqlDAL.DAL
{
    internal class StringContent
    {
        private string v;

        public StringContent(string v)
        {
            this.v = v;
        }

        public static implicit operator HttpContent(StringContent v)
        {
            throw new NotImplementedException();
        }
    }
}