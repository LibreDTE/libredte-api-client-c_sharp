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
        public void TestEmitirDteTemporal() // El diccionario debe seguir una estructura específica.
        {
            TestEnv test_env = new TestEnv();
            test_env.SetVariablesDeEntorno();
            Dte dte = new Dte();

            string receptor = Environment.GetEnvironmentVariable("TEST_RECEPTOR_EMITIR_DTE"); // RUT Receptor con DV
            string tipoDte = Environment.GetEnvironmentVariable("TEST_TIPO_DTE"); // Tipo de DTE (ID como 33, 39, etc)
            string nombreRecep = Environment.GetEnvironmentVariable("TEST_RECEPTOR_NOMBRE_DTE"); // Razón Social Receptor
            string giroRecep = Environment.GetEnvironmentVariable("TEST_REC_GIRO_DTE"); // Giro Receptor (Informática, etc)
            string dirRecep = Environment.GetEnvironmentVariable("TEST_REC_DIR_DTE"); // Dirección Receptor
            string comunaRecep = Environment.GetEnvironmentVariable("TEST_REC_COMUNA_DTE"); // Comuna Receptor
            string emisor = Environment.GetEnvironmentVariable("TEST_EMISOR_DTE"); // RUT Emisor sin DV

            // ARMAR DICCIONARIO

            // CREACIÓN DE ENCABEZADO
            // Tipo de Dte
            Dictionary<string, int> IdDoc = new Dictionary<string, int>()
            {
                {"TipoDTE", Convert.ToInt32(tipoDte)}
            };
            // Emisor
            Dictionary<string, string> Emisor = new Dictionary<string, string>()
            {
                {"RUTEmisor", emisor}
            };
            // Receptor
            Dictionary<string, string> Receptor = new Dictionary<string, string>()
            {
                {"RUTRecep", receptor},
                {"RznSocRecep", nombreRecep},
                {"GiroRecep", giroRecep},
                {"DirRecep", dirRecep},
                {"CmnaRecep", comunaRecep}
            };
            // Encabezado completo
            Dictionary<string, object> Encabezado = new Dictionary<string, object>()
            {
                {"IdDoc", IdDoc},
                {"Emisor", Emisor},
                {"Receptor", Receptor}
            };

            // CREACIÓN DE DETALLE
            // Items
            Dictionary<string, object> item1 = new Dictionary<string, object>()
            {
                {"IndExe", 0},
                {"NmbItem", "Producto Afecto"},
                {"QtyItem", 1},
                {"PrcItem", 1000}
            };
            Dictionary<string, object> item2 = new Dictionary<string, object>()
            {
                {"IndExe", 1},
                {"NmbItem", "Producto Afecto"},
                {"QtyItem", 1},
                {"PrcItem", 2500}
            };
            Dictionary<string, object> item3 = new Dictionary<string, object>()
            {
                {"IndExe", 2},
                {"NmbItem", "Producto Afecto"},
                {"QtyItem", 2},
                {"PrcItem", 700}
            };
            // Lista de items
            List<Dictionary<string, object>> Detalle = new List<Dictionary<string, object>>()
            {
                item1, item2, item3
            };

            // DTE TEMPORAL COMPLETO
            Dictionary<string, object> dteTemporal = new Dictionary<string, object>()
            {
                {"Encabezado", Encabezado},
                {"Detalle", Detalle}
            };

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


        // Todo lo relacionado con DTEEmitidos ha sido eliminado de pruebas.
    }
}
