using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using libredte.api_client;
using libredte.api_client.utils;
using System.Collections.Generic;

namespace tests
{
    [TestClass]
    public class TestSistema
    {
        [TestMethod]
        public void TestGetMonedaCambios() // DESCARTAR PRUEBA.
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            Sistema sistema = new Sistema();
            string moneda = "CLP";
            string dia = "2023-07-11";


            try
            {
                HttpResponseMessage response = sistema.GetMonedaCambios(moneda, dia);
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
                throw new ApiException($"No se ha podido acceder a la API. Error: {e}");
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
