using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Test.Framework.WebApi.OAuth
{
    public class SigningCredentialsProvider : ISigningCredentialsProvider
    {
        public SigningCredentialsProvider()
        {
        }

        public SigningCredentials GetSigningCredentials(string issuer, string audience)
        {
            if (issuer != JwtWebApiConstants.Jwt.IssuerName ||
                audience != JwtWebApiConstants.Jwt.AllowedAudience)
                return null;

            var symmetricKey = Convert.FromBase64String(JwtWebApiConstants.Jwt.SigningKey);
            //var symmetricKey = new RandomBufferGenerator(256 / 8).GenerateBufferFromSeed(256 / 8);
            return new SigningCredentials(
                        new InMemorySymmetricSecurityKey(symmetricKey),
                        "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                        "http://www.w3.org/2001/04/xmlenc#sha256");
        }

        //public SigningCredentials GetSigningCredentials(string issuer, string audience)
        //{
        //    RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
        //    RsaKeyGenerationResult keyGenerationResult = GenerateRsaKeys();
        //    publicAndPrivate.FromXmlString(keyGenerationResult.PublicAndPrivateKey);

        //    return new SigningCredentials(new RsaSecurityKey(publicAndPrivate), SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest);
        //}

        //public SigningCredentials GetSigningCredentials(string issuer, string audience)
        //{
        //    var cert = X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=as.local", false).First();
        //    return new X509SigningCredentials(cert);
        //}

        private static RsaKeyGenerationResult GenerateRsaKeys()
        {
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider(2048);
            RSAParameters publicKey = myRSA.ExportParameters(true);
            RsaKeyGenerationResult result = new RsaKeyGenerationResult();
            result.PublicAndPrivateKey = myRSA.ToXmlString(true);
            result.PublicKeyOnly = myRSA.ToXmlString(false);
            return result;
        }
    }
    public class RsaKeyGenerationResult
    {
        public string PublicKeyOnly { get; set; }
        public string PublicAndPrivateKey { get; set; }
    }
    public class RandomBufferGenerator
    {
        private readonly Random _random = new Random();
        private readonly byte[] _seedBuffer;

        public RandomBufferGenerator(int maxBufferSize)
        {
            _seedBuffer = new byte[maxBufferSize];

            _random.NextBytes(_seedBuffer);
        }

        public byte[] GenerateBufferFromSeed(int size)
        {
            int randomWindow = _random.Next(0, size);

            byte[] buffer = new byte[size];

            Buffer.BlockCopy(_seedBuffer, randomWindow, buffer, 0, size - randomWindow);
            Buffer.BlockCopy(_seedBuffer, 0, buffer, size - randomWindow, randomWindow);

            return buffer;
        }
    }
}