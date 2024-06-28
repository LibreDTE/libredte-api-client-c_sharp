using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    class TestEnv_dist
    {
        public TestEnv_dist()
        {

        }

        public void SetVariablesDeEntorno()
        {
            // Variables de entorno LibreDTE
            string LIBREDTE_HASH = ""; // Hash de usuario de LibreDTE
            string LIBREDTE_URL = "https://libredte.cl"; // URL base
            string LIBREDTE_RUT = ""; // RUT de usuario
            string LIBREDTE_AMBIENTE = "1"; // Ambiente SII

            // Variables Cobros
            string TEST_RUT_COBRO = ""; // RUT del emisor del cobro
            string TEST_CODIGO_COBRO = ""; // Código del cobro

            // Variables DTE
            string TEST_TIPO_DTE = ""; // Código del Tipo de DTE
            string TEST_CODIGO_DTE = ""; // Folio del DTE
            string TEST_RECEPTOR_DTE = ""; // RUT del receptor del DTE
            string TEST_EMISOR_DTE = ""; // RUT del emisor del DTE
            string TEST_EMAIL_DTE = ""; // Email que recibirá el DTE

            // Definición de variables de entorno
            Environment.SetEnvironmentVariable("LIBREDTE_HASH", LIBREDTE_HASH);
            Environment.SetEnvironmentVariable("LIBREDTE_URL", LIBREDTE_URL);
            Environment.SetEnvironmentVariable("LIBREDTE_RUT", LIBREDTE_RUT);
            Environment.SetEnvironmentVariable("LIBREDTE_AMBIENTE", LIBREDTE_AMBIENTE);

            // Definición de variables Cobros
            Environment.SetEnvironmentVariable("TEST_RUT_COBRO", TEST_RUT_COBRO);
            Environment.SetEnvironmentVariable("TEST_CODIGO_COBRO", TEST_CODIGO_COBRO);

            // Definición de variables DTE
            Environment.SetEnvironmentVariable("TEST_TIPO_DTE", TEST_TIPO_DTE);
            Environment.SetEnvironmentVariable("TEST_CODIGO_DTE", TEST_CODIGO_DTE);
            Environment.SetEnvironmentVariable("TEST_RECEPTOR_DTE", TEST_RECEPTOR_DTE);
            Environment.SetEnvironmentVariable("TEST_EMISOR_DTE", TEST_EMISOR_DTE);
            Environment.SetEnvironmentVariable("TEST_EMAIL_DTE", TEST_EMAIL_DTE);
        }
    }
}
