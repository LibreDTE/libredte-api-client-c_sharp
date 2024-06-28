using libredte.api_client;
using libredte.api_client.utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace tests
{
    [TestClass]
    public class TestCobros
    {
        [TestMethod]
        public void TestGetCobros()
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string rut = Environment.GetEnvironmentVariable("TEST_EMISOR_DTE");
            Dictionary<string, object> cobro = new Dictionary<string, object>(); // ARMAR EL DICCIONARIO
            Cobros cobros = new Cobros();

            try
            {
                HttpResponseMessage response = cobros.GetCobros(rut, cobro);
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                Trace.WriteLine(jsonResponse.ToString());
                Dictionary<string, object> resultado = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                foreach (var informacion in resultado)
                {
                    Trace.WriteLine(informacion.ToString());
                }
                Assert.AreEqual(resultado.Count > 0, true);
            }
            catch (AssertFailedException e)
            {
                throw new ApiException($"No se ha podido encontrar contribuyente. Error: {e}");
            }
            catch (JsonSerializationException e)
            {
                throw new ApiException($"Error de serialización json. Error: {e}");
            }
            catch (Exception e)
            {
                throw new ApiException($"Error: {e}");
            }
        }

        [TestMethod]
        public void TestGetCobroDteTemporal()
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string receptor = Environment.GetEnvironmentVariable("TEST_RECEPTOR_DTE");
            string tipoDte = Environment.GetEnvironmentVariable("TEST_TIPO_DTE");
            string codigo = Environment.GetEnvironmentVariable("TEST_CODIGO_DTE");
            string emisor = Environment.GetEnvironmentVariable("TEST_EMISOR_DTE");
            Cobros cobros = new Cobros();

            try
            {
                HttpResponseMessage response = cobros.GetCobroDteTemporal(receptor, tipoDte, codigo, emisor);
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                Trace.WriteLine(jsonResponse.ToString());
                Dictionary<string, object> resultado = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                foreach (var informacion in resultado)
                {
                    Trace.WriteLine(informacion.ToString());
                }
                Assert.AreEqual(resultado.Count > 0, true);
            }
            catch (AssertFailedException e)
            {
                throw new ApiException($"No se ha podido encontrar contribuyente. Error: {e}");
            }
            catch (JsonSerializationException e)
            {
                throw new ApiException($"Error de serialización json. Error: {e}");
            }
            catch (Exception e)
            {
                throw new ApiException($"Error: {e}");
            }
        }

        [TestMethod]
        public void TestGetCobroInfo()
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string codigo = Environment.GetEnvironmentVariable("TEST_CODIGO_COBRO");
            string rut = Environment.GetEnvironmentVariable("TEST_RUT_COBRO");
            Cobros cobros = new Cobros();

            try
            {
                HttpResponseMessage response = cobros.GetCobroInfo(codigo, rut);
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                Trace.WriteLine(jsonResponse.ToString());
                Dictionary<string, object> resultado = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                foreach (var informacion in resultado)
                {
                    Trace.WriteLine(informacion.ToString());
                }
                Assert.AreEqual(resultado.Count > 0, true);
            }
            catch (AssertFailedException e)
            {
                throw new ApiException($"No se ha podido encontrar contribuyente. Error: {e}");
            }
            catch (JsonSerializationException e)
            {
                throw new ApiException($"Error de serialización json. Error: {e}");
            }
            catch (Exception e)
            {
                throw new ApiException($"Error: {e}");
            }
        }
    }
}
