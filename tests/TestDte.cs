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
    public class TestDte
    {
        [TestMethod]
        public void TestEmitirDteTemporal()
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            Dte dte = new Dte();
            // Reemplazar líneas de código por un valor más "completo" o "real", de paso usar variables de entorno
            Dictionary<string, object> dteTemporal = new Dictionary<string, object>();

            try
            {
                HttpResponseMessage response = dte.EmitirDteTemporal(dteTemporal);
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
        public void TestGetDteTemporal()
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string receptor = Environment.GetEnvironmentVariable("TEST_RECEPTOR_DTE");
            string tipoDte = Environment.GetEnvironmentVariable("TEST_TIPO_DTE");
            string codigo = Environment.GetEnvironmentVariable("TEST_CODIGO_DTE");
            string emisor = Environment.GetEnvironmentVariable("TEST_EMISOR_DTE");

            Dte dte = new Dte();

            try
            {
                HttpResponseMessage response = dte.GetDteTemporal(receptor, tipoDte, codigo, emisor);
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
        public void TestDteTemporalEnviarEmail()
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string receptor = Environment.GetEnvironmentVariable("TEST_RECEPTOR_DTE");
            string tipoDte = Environment.GetEnvironmentVariable("TEST_TIPO_DTE");
            string codigo = Environment.GetEnvironmentVariable("TEST_CODIGO_DTE");
            string emisor = Environment.GetEnvironmentVariable("TEST_EMISOR_DTE");
            string email = Environment.GetEnvironmentVariable("TEST_EMAIL_DTE");
            List<string> emails = new List<string>();
            emails.Add(email);
            Dictionary<string, object> dataEmail = new Dictionary<string, object>() // Buscar cómo completar el diccionario email
            {
                {"emails", email },
                {"asunto", "Test email 3" },
                {"mensaje", "Prueba correo 3" },
                {"cotizacion", true }
            };

            Dte dte = new Dte();

            try
            {
                HttpResponseMessage response = dte.DteTemporalEnviarEmail(receptor, tipoDte, codigo, emisor, dataEmail);
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                Trace.WriteLine(jsonResponse.ToString());
                // Dictionary<string, object> resultado = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);
                
                Assert.AreEqual(jsonResponse.Length > 0, true);
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
        public void TestGetDteEmitidos() // CREO QUE ESTE NO SE PUEDE PROBAR
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string rut = Environment.GetEnvironmentVariable("TEST_EMISOR_DTE");
            Dictionary<string, object> filtros = new Dictionary<string, object>()
            {
                {"receptor", null },
                {"razon_social", null },
                {"dte", null },
                {"folio", null },
                {"fecha", null },
                {"total", null },
                {"usuario", null },
                {"fecha_desde", null },
                {"fecha_hasta", null },
                {"total_desde", null },
                {"total_hasta", null },
                {"sucursal_sii", null },
                {"periodo", null },
                {"receptor_evento", null },
                {"cedido", null },
                {"xml", new Dictionary<string, string>()
                {
                    {"Detalle/NmbItem", "abono" }
                } }
            };
            /*
            "receptor": null,
	        "razon_social": null,
	        "dte": null,
	        "folio": null,
	        "fecha": null,
	        "total": null,
	        "usuario": null,
	        "fecha_desde": null,
	        "fecha_hasta": null,
	        "total_desde": null,
	        "total_hasta": null,
	        "sucursal_sii": null,
	        "periodo": null,
	        "receptor_evento": null,
	        "cedido": null,
	        "xml": {
                "Detalle/NmbItem": "abono"
            }
            */

            Dte dte = new Dte();

            try
            {
                HttpResponseMessage response = dte.GetDteEmitidos(rut, filtros);
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

        /*
        [TestMethod]
        public void TestDteEmitidosActualizarEstado() // CREO QUE NO SE PUEDE PROBAR
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            // Variables de entorno definidas desde test_env
            string rut = Environment.GetEnvironmentVariable("LIBREDTE_RUT");
            string dteEmitido = "";
            string folio = "";

            Dte dte = new Dte();

            try
            {
                HttpResponseMessage response = dte.DteEmitidosActualizarEstado(dteEmitido, folio, rut);
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
        */
    }
}
