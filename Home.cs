using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EstadisticaDescriptiva
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        #region Variables
        bool empty = true;
        string coleccionBid;
        int ordenadorCol = 0;
        int ordenadorRow = 0;
        int contador = 0;
        int repetidor = 0;
        int mayor = 0;
        int coleccionDato = 0;
        int coleccionY = 0;
        int coleccionX = 0;
        int contadorCol = 0;
        int contadorDatos = 0;
        double N = 0;
        double comparacionFrecAcum = 0;
        double mediana = 0;
        double PartialMe = 0;
        double mediaX = 0;
        double mediaY = 0;
        double desviacionMedia = 0;
        double desviacionTipica = 0;
        double varianza = 0;
        double covarianza = 0;
        double coefcorrelacion = 0;
        int calculo;

        //Estas vriables sirven para trabajar con los límites superior e inferior
        double a = 0;
        bool carga = true;

        //Se elije un caracter delimitador
        char[] delimitador = { ' ', ':', ';', '-', '\\', ',' };
        double amplitud;
        string Lim;
        double Ls;
        double Li;

        #endregion

        #region Metodos
        private void OrdenarDatos()
        {
            switch (calculo)
            {
                case 0:
                    #region Unidimensional
                    HabilitarDGVcoleccion();

                    





                    //string[] datos = txtDatos.Text.Split(delimitador);
                    //string dato;
                    //int count = 0;
                    //int contadorFila = 0;
                    //contadorCol = 1;
                    //int contador = datos.Length;
                    //dgvColeccion.Rows.Add(contador + 1);                    
                    //foreach (var num in datos.GroupBy(x => x))
                    //{
                    //    dato = num.Key;
                    //    count = num.Count();
                    //    dgvColeccion.Rows[contadorFila].Cells[contadorCol + 1].Value = dato;
                    //    dgvColeccion.Rows[contadorFila].Cells[contadorCol + 1].Value = count;
                    //    contadorFila++;
                    //}

                    #endregion
                    break;
                case 1:
                    #region Unidimensional agrupado
                    HabilitarDGVcoleccion();
                    coleccionX = Convert.ToInt32(dgvColeccion.Rows.Count - 1);
                    dgvColeccion.Rows.Add(1);

                    for (int i = 0; i < 1; i++)
                    {
                        for (int j = 0; j < coleccionX; j++)
                        {
                            //Luego paso lo que hay en la celda del intervalo a una variable tipo string.
                            Lim = dgvColeccion.Rows[j].Cells[i].Value.ToString();

                            //Ahora separo los caracteres que tengo por cada demilitador encontrado.
                            string[] intervalo = Lim.Split(delimitador);

                            foreach (var num in intervalo)
                            {
                                //Necesito tener 2 numeros para sumarlos
                                //para luego poder dividirlos
                                // por lo que creo una variable booleana y le voy cambiando el valor

                                if (carga)
                                {
                                    //si es verdadero, le asigno el primer valor y cambio el valor booleano
                                    a = Convert.ToDouble(num);
                                    carga = false;
                                }
                                else
                                {
                                    //si es falso, le sumo el segundo valor al que ya tengo
                                    a += Convert.ToDouble(num);
                                    carga = true;
                                }
                            }

                            //Ahora divido la suma sobre 2 y obtengo el dato x y lo coloco en la siguiente columna
                            dgvColeccion.Rows[j].Cells[1].Value = (a / 2).ToString();
                        }
                    }
                    #endregion
                    break;
                case 2:
                    #region Bidimensional
                    HabilitarDGVBidi();
                    double r = 0;
                    coleccionY = Convert.ToInt32(dgvDatos.Rows.Count - 1);

                    // Este método me cuenta solo las celdas que tienen datos, pero consecutivas.
                    // Si hay una celda vacía, se frena ahí y esa cantidad contada va a ser la cantidad
                    // de columnas que tenga de datos.

                    do
                    {
                        coleccionBid = dgvDatos.Rows[0].Cells[coleccionX].Value.ToString();
                        coleccionX++;

                    } while (dgvDatos.Rows[0].Cells[coleccionX].Value != null);
                    coleccionBid = string.Empty;

                    //

                    dgvBidimensional.Rows.Add(coleccionX + 1);
                    N = coleccionX;
                    txtEne.Text = N.ToString();

                    for (int i = 0; i < coleccionY; i++)
                    {
                        for (int j = 0; j < coleccionX; j++)
                        {
                            coleccionBid = dgvDatos.Rows[i].Cells[j].Value.ToString();
                            dgvBidimensional.Rows[j].Cells[i].Value = coleccionBid;
                            r += Convert.ToInt32(coleccionBid);
                        }
                        dgvBidimensional.Rows[coleccionX].Cells[i].Value = r.ToString();
                        r = 0;
                        coleccionBid = string.Empty;
                    }
                    #endregion
                    break;
            }
        }
        private void FrecuenciaAbsoluta()
        {
            switch (calculo)
            {
                case 0:
                    #region Unidimensional
                    //for (int x = 0; contador < mayor; x++)
                    //{
                    //    for (int i = 0; i < coleccionY; i++)
                    //    {
                    //        for (int j = 0; j < (coleccionX - 1); j++)
                    //        {
                    //            if (dgvDatos.Rows[i].Cells[j].Value != null)
                    //            {
                    //                coleccionDato = Convert.ToInt32(dgvDatos.Rows[i].Cells[j].Value);

                    //                if (contador == coleccionDato)
                    //                {
                    //                    repetidor++;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    // hacer que mientras un dato no se repita, no se escriba en el dgv

                    //    if (repetidor > 0)
                    //    {
                    //        dgvColeccion.Rows[ordenadorRow].Cells[1].Value = contador.ToString();
                    //        dgvColeccion.Rows[ordenadorRow].Cells[2].Value = repetidor.ToString();
                    //        //sumatoria de las frecuencias
                    //        N += repetidor;
                    //        repetidor = 0;
                    //        contador++;
                    //        ordenadorRow++;
                    //    }
                    //}
                    //coleccionY = 0;
                    //coleccionX = 0;
                    //coleccionDato = 0;
                    //txtEne.Text = N.ToString();
                    //dgvColeccion.Rows[ordenadorRow].Cells[2].Value = N.ToString();
                    //ordenadorRow = 0;
                    #endregion
                    break;
                case 1:
                    #region Unidimensional agrupado

                    for (int i = 0; i < coleccionX; i++)
                    {
                        N += Convert.ToDouble(dgvColeccion.Rows[i].Cells[2].Value);
                    }

                    dgvColeccion.Rows[coleccionX].Cells[2].Value = N.ToString();
                    txtEne.Text = N.ToString();

                    #endregion
                    break;
                case 2:
                    #region Bidimensional

                    #endregion
                    break;
            }

        }
        private void FrecuenciaAcumulada()
        {
            int reemplazo;
            int reemplazo2;

            switch (calculo)
            {
                case 0:
                    #region Unidimensional
                    for (int i = 0; i < mayor; i++)
                    {
                        if (ordenadorRow == 0)
                        {
                            dgvColeccion.Rows[ordenadorRow].Cells[3].Value = dgvColeccion.Rows[ordenadorRow].Cells[2].Value;
                        }
                        else
                        {
                            reemplazo = Convert.ToInt32(dgvColeccion.Rows[ordenadorRow - 1].Cells[3].Value);
                            reemplazo2 = Convert.ToInt32(dgvColeccion.Rows[ordenadorRow].Cells[2].Value);
                            dgvColeccion.Rows[ordenadorRow].Cells[3].Value = (reemplazo + reemplazo2).ToString();
                        }
                        ordenadorRow++;
                    }
                    ordenadorRow = 0;
                    #endregion
                    break;

                case 1:
                    #region Unidimensional agrupados
                    for (int i = 0; i < coleccionX; i++)
                    {
                        if (i == 0)
                        {
                            dgvColeccion.Rows[i].Cells[3].Value = dgvColeccion.Rows[i].Cells[2].Value;
                        }
                        else
                        {
                            reemplazo = Convert.ToInt32(dgvColeccion.Rows[i - 1].Cells[3].Value);
                            reemplazo2 = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                            dgvColeccion.Rows[i].Cells[3].Value = (reemplazo + reemplazo2).ToString();
                        }
                    }

                    #endregion
                    break;
                //no se si lo necesito a este case, pero por las dudas lo tengo listo.
                case 2:
                    #region Bidimensional

                    #endregion
                    break;
            }
        }
        private void Mediana()
        {
            if ((N % 2) == 0)
            {
                mediana = N / 2;
            }
            else
            {
                mediana = (N + 1) / 2;
            }
            CompararDatosMediana();
        }
        private void CompararDatosMediana()
        {
            int i = 0;
            do
            {
                comparacionFrecAcum = Convert.ToDouble(dgvColeccion.Rows[i].Cells[3].Value);
                PartialMe = comparacionFrecAcum - mediana;
                if (PartialMe < 0)
                {

                }
                i++;
            } while (PartialMe < 0);
            switch (calculo)
            {
                case 0:
                    #region Unidimensional

                    mediana = Convert.ToDouble(dgvColeccion.Rows[i].Cells[1].Value);

                    while ((Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value)) == 0)
                    {
                        i++;
                    }
                    mediana += Convert.ToDouble(dgvColeccion.Rows[i].Cells[1].Value);
                    mediana = mediana / 2;
                    txtMediana.Text = mediana.ToString();
                    #endregion
                    break;
                case 1:
                    #region Unidimensional Agrupados

                    //Luego paso lo que hay en la celda del intervalo a una variable tipo string.
                    Lim = dgvColeccion.Rows[i].Cells[0].Value.ToString();

                    //Ahora separo los caracteres que tengo por cada demilitador encontrado.
                    string[] intervalo = Lim.Split(delimitador);

                    foreach (var num in intervalo)
                    {
                        //obtengo los límites para realizar el cálculo de la mediana
                        if (carga)
                        {
                            Li = Convert.ToDouble(num);
                            carga = false;
                        }
                        else
                        {
                            Ls = Convert.ToDouble(num);
                            carga = true;
                        }
                    }
                    txtIntervaloMediana.Text = "El intervalo de la mediana es: " + Lim;

                    amplitud = Ls - Li;
                    txtAmplitud.Text = amplitud.ToString();

                    double F = Convert.ToDouble(dgvColeccion.Rows[i - 1].Cells[3].Value); // F
                    double f = Convert.ToDouble(dgvColeccion.Rows[i].Cells[2].Value); // f
                    mediana = Li + ((mediana - F) / f) * amplitud;
                    txtMediana.Text = mediana.ToString();

                    #endregion
                    break;
                case 2:
                    break;
            }
        }
        private void Media()
        {
            int resAcum = 0;
            int resMulti = 0;
            int resSumatoria = 0;

            switch (calculo)
            {
                case 0:
                    #region Simples
                    for (int i = 0; i < mayor; i++)
                    {
                        for (int j = 1; j < 2; j++)
                        {
                            resMulti = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value);
                            resAcum = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                            resAcum = resAcum * resMulti; //Esto es la multiplicación x por f. A cada variable le asigno un dato y multiplico
                            resSumatoria += resAcum; //Acá está la sumatoria
                            dgvColeccion.Rows[i].Cells[4].Value = resAcum.ToString(); //y acá muestro el resultado
                            resMulti = 0;
                            resAcum = 0;
                        }
                    }
                    dgvColeccion.Rows[mayor].Cells[4].Value = resSumatoria.ToString();

                    mediaX = resSumatoria / N;
                    txtMedia.Text = mediaX.ToString("N2");
                    #endregion
                    break;
                case 1:

                    for (int i = 0; i < coleccionX; i++)
                    {
                        for (int j = 1; j < 2; j++)
                        {
                            resMulti = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value);
                            resAcum = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                            resAcum = resAcum * resMulti; //Esto es la multiplicación x por f. A cada variable le asigno un dato y multiplico
                            resSumatoria += resAcum; //Acá está la sumatoria
                            dgvColeccion.Rows[i].Cells[4].Value = resAcum.ToString(); //y acá muestro el resultado
                            resMulti = 0;
                            resAcum = 0;
                        }
                    }
                    dgvColeccion.Rows[coleccionX].Cells[4].Value = resSumatoria.ToString();

                    mediaX = resSumatoria / N;
                    txtMedia.Text = mediaX.ToString("N2");
                    break;
                case 2:
                    #region Bidimensional
                    int x = 0;
                    int y = 0;

                    x = Convert.ToInt32(dgvBidimensional.Rows[coleccionX].Cells[0].Value);
                    y = Convert.ToInt32(dgvBidimensional.Rows[coleccionX].Cells[1].Value);

                    mediaX = Convert.ToDouble(x) / N;
                    mediaY = Convert.ToDouble(y) / N;

                    txtMediaX.Text = mediaX.ToString("N2");
                    txtMediaY.Text = mediaY.ToString("N2");

                    #endregion
                    break;
            }
        }
        private void Moda()
        {
            //El que se repite más veces
            int frecuenciaModa = 0;
            int datoModa = 0;
            int datoModa2 = 0;
            int moda = 0;
            bool bimodal = false;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    //Acá es sencillo. Reviso toda la columna de frecuencias
                    //(por eso el 1 porque ese N° no cambia es la misma columna)
                    //esta variable la inicio en 0 y voy comparando si el dato que veo de la frec
                    //es mayor.
                    frecuenciaModa = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                    if (frecuenciaModa > moda) //Si la frecuencia que voy viendo es mayor que el N° que tengo almacenada en "moda"
                    {
                        moda = frecuenciaModa; // entonces lo reemplazo.
                        datoModa = Convert.ToInt32(dgvColeccion.Rows[i].Cells[1].Value); //y me guardo el dato x de esa fila.
                    }
                }
            }

            for (int i = 0; i < mayor; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    frecuenciaModa = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                    if (frecuenciaModa == moda)
                    {
                        if (datoModa != (Convert.ToInt32(dgvColeccion.Rows[i].Cells[1].Value)))
                        {
                            datoModa2 = Convert.ToInt32(dgvColeccion.Rows[i].Cells[1].Value);
                            bimodal = true;
                        }
                    }
                }
            }

            if (bimodal)
            {
                txtModa.Text = "Es bimodal con los datos " + datoModa + " y " + datoModa2 + " repetidos " + moda + " veces.";
            }
            else
            {
                //dato x                                                   //frecuencia
                txtModa.Text = "El dato " + datoModa.ToString() + " tiene la mayor frecuencia con: " + moda.ToString();
            }
        }
        private void DesviacionMedia()
        {
            //restar x a la media y luego multiplicar por f. La sumatoria se divide por N
            double x = 0;
            double f = 0;
            double r = 0;
            double sum = 0;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    x = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value);
                    f = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);
                    r = (x - mediaX) * f;
                    sum += r;
                    dgvColeccion.Rows[i].Cells[6].Value = r.ToString("N2");
                    x = 0;
                    f = 0;
                    r = 0;
                }
            }
            dgvColeccion.Rows[mayor].Cells[6].Value = sum.ToString("N2");

            desviacionMedia = (sum / N); //es un tema hacer potencias acá así que solo la multiplicamos así.

            txtDesvMedia.Text = desviacionMedia.ToString("N2");
        }
        private void Varianza()
        {
            switch (calculo)
            {
                case 0:
                    #region unidimensional
                    VarianzaUnidimensional();
                    #endregion
                    break;
                case 1:
                    #region Unidimensional Agrupada
                    VarianzaUnidimensional();
                    #endregion
                    break;
                case 2:
                    #region Bidimensional

                    double sumaX = Convert.ToDouble(dgvBidimensional.Rows[coleccionX].Cells[3].Value);
                    double sumaY = Convert.ToDouble(dgvBidimensional.Rows[coleccionX].Cells[4].Value);
                    txtVarianzaX.Text = ((sumaX / N) - (mediaX * mediaX)).ToString("N3");
                    txtVarianzaY.Text = ((sumaY / N) - (mediaY * mediaY)).ToString("N3");

                    #endregion
                    break;
            }
        }
        private void VarianzaUnidimensional()
        {
            int resAcum = 0;
            int resMulti = 0;
            double resSumatoria = 0;
            for (int i = 0; i < mayor; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    resMulti = Convert.ToInt32(dgvColeccion.Rows[i].Cells[j].Value); //Esto es x de cada fila
                    resAcum = Convert.ToInt32(dgvColeccion.Rows[i].Cells[2].Value);  //Esto es f de cada fila
                    resAcum = (resMulti * resMulti) * resAcum;  //Acá está x al cuadrado por f
                    resSumatoria += Convert.ToDouble(resAcum); //Esto es la sumatora de x al cuadrado por f
                    dgvColeccion.Rows[i].Cells[5].Value = resAcum.ToString();
                    resMulti = 0;
                    resAcum = 0;
                }
            }
            dgvColeccion.Rows[mayor].Cells[5].Value = resSumatoria.ToString();

            varianza = (resSumatoria / N) - (mediaX * mediaX); //es un tema hacer potencias acá así que solo la multiplicamos así.

            txtVarianza.Text = varianza.ToString("N3");
        }
        private void DesviacionTipica()
        {
            switch (calculo)
            {
                case 0:
                    desviacionTipica = Math.Sqrt(varianza);
                    txtDesviacionTipica.Text = desviacionTipica.ToString("N3");
                    break;
                case 1:
                    break;
                case 2:
                    #region Bidimensional
                    txtDesvTipicaX.Text = (Math.Sqrt(Convert.ToDouble(txtVarianzaX.Text))).ToString("N3");
                    txtDesvTipicaY.Text = (Math.Sqrt(Convert.ToDouble(txtVarianzaY.Text))).ToString("N3");
                    #endregion
                    break;
            }
        }
        private void CoefVariacion()
        {
            txtCoefVariacion.Text = (desviacionTipica / mediaX).ToString("N3");
        }
        private void Percentil()
        {
            double p = Convert.ToInt32(txtPercentil.Text);
            txtPerRespuesta.Text = ((p * N) / 100).ToString();
        }
        private void Quartil()
        {
            int q = Convert.ToInt32(txtQuartil.Text);
            txtQuarRespuesta.Text = ((q * N) / 4).ToString();
        }
        private void Decil()
        {
            int d = Convert.ToInt32(txtDecil.Text);
            txtDecilRespuesta.Text = ((d * N) / 10).ToString();
        }
        private void Covarianza()
        {
            double x = 0;
            double y = 0;
            double multi = 0;
            double sum = 0;
            for (int i = 0; i < coleccionX; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    x = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[j].Value);
                    y = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[1].Value);
                    multi = x * y;
                    sum += multi;
                    dgvBidimensional.Rows[i].Cells[2].Value = multi.ToString();
                    y = 0;
                    x = 0;
                    multi = 0;
                }
            }
            dgvBidimensional.Rows[coleccionX].Cells[2].Value = sum.ToString();

            double xy = mediaX * mediaY;
            double rs = sum / N;
            covarianza = rs - xy;
            txtCovarianza.Text = (covarianza).ToString("N3");

            rs = 0;
            xy = 0;
            sum = 0;
        }
        private void CoeficienteCorrelacion()
        {
            int x = 0;
            int multi = 0;
            int sum = 0;
            double desvTipX = 0;
            double desvTipY = 0;
            int col = 3;

            //X al cuadrado y su sumatoria
            for (int i = 0; i < coleccionX; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    x = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[j].Value);
                    multi = x * x;
                    sum += multi;
                    dgvBidimensional.Rows[i].Cells[col].Value = multi.ToString();
                    x = 0;
                    multi = 0;
                }
            }
            dgvBidimensional.Rows[coleccionX].Cells[col].Value = sum.ToString();
            col++;

            desvTipX = Convert.ToDouble((sum / N) - (mediaX * mediaX));

            x = 0;
            multi = 0;
            sum = 0;

            //Y al cuadrado y su sumatoria.
            for (int i = 0; i < coleccionX; i++)
            {
                for (int j = 1; j < 2; j++)
                {
                    x = Convert.ToInt32(dgvBidimensional.Rows[i].Cells[j].Value);
                    multi = x * x;
                    sum += multi;
                    dgvBidimensional.Rows[i].Cells[col].Value = multi.ToString();
                    x = 0;
                    multi = 0;
                }
            }
            dgvBidimensional.Rows[coleccionX].Cells[col].Value = sum.ToString();

            desvTipY = Convert.ToDouble((sum / N) - (mediaY * mediaY));

            desvTipX = Math.Sqrt(desvTipX);
            desvTipY = Math.Sqrt(desvTipY);

            //Coeficiente correlación:
            coefcorrelacion = covarianza / (desvTipX * desvTipY);
            txtCoefCorre.Text = coefcorrelacion.ToString("N4");

            x = 0;
            multi = 0;
            sum = 0;
            col = 3;

        }
        private void ChequearDGV()
        {
            if (dgvDatos.Rows.Count > 0 && chkUnidimensional.Checked == true)
            {
                empty = false;
            }
            else
            {
                if (dgvColeccion.Rows.Count > 0 && chkUniAgrupado.Checked == true)
                {
                    empty = false;
                }
                else
                {
                    if (dgvDatos.Rows.Count > 0 && chkBidimensional.Checked == true)
                    {
                        empty = false;
                    }
                    else
                    {
                        empty = true;
                    }
                }
            }
        }

        #endregion

        #region Pasar datos entre DGV
        private void HabilitarDGVcoleccion()
        {
            dgvBidimensional.Visible = false;
            dgvColeccion.Visible = true;
        }
        private void HabilitarDGVBidi()
        {
            dgvBidimensional.Visible = true;
            dgvColeccion.Visible = false;
        }

        #endregion

        #region Reset de controles

        private void LimpiarDGV()
        {
            foreach (var dgv in this.Controls.OfType<DataGridView>())
            {
                dgv.Rows.Clear();
            }
        }
        private void LimpiarTxt()
        {
            /*Estos medotos buscan el tipo de control dentro del form o
             dentro de otro control y realiza las acciones que se le pidan
            En este caso, es vaciar el text de los textbox*/
            foreach (var txt in this.Controls.OfType<TextBox>())
            {
                txt.Text = string.Empty;
            }
            foreach (var txt in gpBidimensional.Controls.OfType<TextBox>())
            {
                txt.Text = string.Empty;
            }
        }
        private void LimpiarChk()
        {
            // acá los checkbox
            foreach (var chk in gpdispersion.Controls.OfType<CheckBox>())
            {
                chk.Checked = false;//Les quito el check
            }
            foreach (var chk in gpCentralizacion.Controls.OfType<CheckBox>())
            {
                chk.Checked = false;
            }
            foreach (var chk in gpBidimensional.Controls.OfType<CheckBox>())
            {
                chk.Checked = false;
            }
        }
        private void ResetVariables()
        {
            ordenadorCol = 0;
            ordenadorRow = 0;
            contador = 0;
            repetidor = 0;
            mayor = 0;
            coleccionDato = 0;
            coleccionY = 0;
            coleccionX = 0;
            N = 0;
            comparacionFrecAcum = 0;
            mediana = 0;
            PartialMe = 0;
            mediaX = 0;
            mediaY = 0;
            desviacionMedia = 0;
            desviacionTipica = 0;
            varianza = 0;
            covarianza = 0;
            coefcorrelacion = 0;
            a = 0;
            carga = true;
            amplitud = 0;
            Lim = string.Empty;
            Ls = 0;
            Li = 0;
        }
        private void QuitarChecks()
        {
            foreach (var chk in gpdispersion.Controls.OfType<CheckBox>())
            {
                chk.Checked = false;
                chk.Enabled = false;
            }
        }

        #endregion        

        #region Eventos
        private void btnCalcularOrdenar_Click(object sender, EventArgs e)
        {
            ChequearDGV();
            if (empty)
            {
                MessageBox.Show("No hay datos ingresados", "Grillas vacías", MessageBoxButtons.OK);
            }
            else
            {
                if (btnCalcularOrdenar.Text == "Limpiar")
                {
                    LimpiarChk();
                    LimpiarDGV();
                    LimpiarTxt();
                    ResetVariables();
                    btnCalcularOrdenar.Text = "Calcular";
                }
                else
                {
                    OrdenarDatos();
                    FrecuenciaAbsoluta();
                    //FrecuenciaAcumulada();
                    //Mediana();
                    //Media();
                    //Moda();
                    btnCalcularOrdenar.Text = "Limpiar";
                }
            }
        }
        private void btnMediana_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnMediana.Checked)
            {
                txtMediana.Text = string.Empty;
            }
            else
            {
                Mediana(); //Acá llamo al método encapsulado para calcular la mediana
            }
        }
        private void btnMedia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnidimensional.Checked)
            {
                if (!btnMedia.Checked)
                {
                    QuitarChecks();
                    txtVarianza.Text = string.Empty;
                    txtDesvMedia.Text = string.Empty;
                    txtMedia.Text = string.Empty;//Así que si no está habilitada y calculada la Media, el chk de la varianza se quita
                }                                //y al txt de la Media se le quita el contenido.
                else
                {
                    Media();//Acá llamo al método encapsulado para calcular la media
                    btnVarianza.Enabled = true; //habilito el chk de la varianza
                    chkDesviacionMedia.Enabled = true;
                }
            }
            else
            {
                Media();
            }


        }
        private void btnModa_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnModa.Checked) //el signo ! significa la negación: Si btnModa no esta con el check entonces ..
            {
                txtModa.Text = string.Empty;
            }
            else
            {
                //por si alguien ignoraba esta opción de VS.
                Moda(); //llamo al método de la moda (Ctrl + click izquierdo) y los lleva al médoto en cuestión)
            }
        }
        private void btnVarianza_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnVarianza.Checked)
            {
                btnTipica.Enabled = false;   //Esto es lo mismo que con la media. Para calcular la tipica, tengo que tener la varianza
                txtVarianza.Text = string.Empty; //sino la saqué, entonces no se habilita el chk de tipica
                btnTipica.Checked = false;
                txtDesviacionTipica.Text = string.Empty;
            }
            else
            {
                Varianza(); //si tiene la varianza, hacemos la misma
                btnTipica.Enabled = true; // y habilitamos la tipica para calcularla.
            }
        }
        private void btnTipica_CheckedChanged(object sender, EventArgs e)
        {
            if (!btnTipica.Checked)
            {
                txtDesviacionTipica.Text = string.Empty;
                chkCoefVariación.Enabled = false;
                txtCoefVariacion.Text = string.Empty;
            }
            else
            {
                chkCoefVariación.Enabled = true;
                DesviacionTipica(); //calculo la desv tipica.
            }
        }
        private void txtPercentil_Leave(object sender, EventArgs e)
        {
            Percentil();
        }
        private void txtQuartil_Leave(object sender, EventArgs e)
        {
            Quartil();
        }
        private void txtDecil_Leave(object sender, EventArgs e)
        {
            Decil();
        }
        private void chkSimple_CheckedChanged(object sender, EventArgs e)
        {
            calculo = 0;
            gpBidimensional.Enabled = false;
            btnMediana.Enabled = true;
        }
        private void chkCompuesto_CheckedChanged(object sender, EventArgs e)
        {
            calculo = 1;
            gpBidimensional.Enabled = false;
            btnMediana.Enabled = true;
        }
        private void btnBidimensional_CheckedChanged(object sender, EventArgs e)
        {
            calculo = 2;
            gpBidimensional.Enabled = true;
            btnMediana.Enabled = false;
        }
        private void chkDesviacionMedia_CheckedChanged(object sender, EventArgs e)
        {
            DesviacionMedia();
        }
        private void chkCoefVariación_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkDesviacionMedia.Checked)
            {
                txtCoefVariacion.Text = string.Empty;
            }
            else
            {
                CoefVariacion();
            }
        }
        private void btnCovarianza_CheckedChanged(object sender, EventArgs e)
        {
            Covarianza();
        }
        private void btnCoefCor_CheckedChanged(object sender, EventArgs e)
        {
            CoeficienteCorrelacion();
        }
        #endregion
    }
}