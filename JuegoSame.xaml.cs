using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LenguajeProgramacion
{
    /// <summary>
    /// Lógica de interacción para JuegoSame.xaml
    /// </summary>
    public partial class JuegoSame : Window
    {
        List<Circulo> circulos;
        int filas = 0;
        int columnas = 0;
        int colores = 0;
        public JuegoSame()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            colores = 5;
            filas = 12;
            columnas = 17;
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            CanvasContenedor.Children.Clear();
            circulos = new List<Circulo>();
            Random rnd = new Random();
            int w =0;
            if (columnas >= filas)
                w = (int)CanvasContenedor.Width / columnas;
            else
                w = (int)CanvasContenedor.Height / filas;
            int x = 0;// (int)CanvasContenedor.Width;
            int y = (int)CanvasContenedor.Height;
            int id = 1;
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    //Random rnd = new Random();
                    int n = rnd.Next(0, colores);
                    Ellipse elipse = new Ellipse();
                    elipse.Width = w;
                    elipse.Height = w;
                    elipse.StrokeThickness = 0;
                    elipse.Opacity = 0.5;
                    switch (n)
                    {
                        case 0: elipse.Fill = new SolidColorBrush(Colors.Blue); break;
                        case 1: elipse.Fill = new SolidColorBrush(Colors.White); break;
                        case 2: elipse.Fill = new SolidColorBrush(Colors.Yellow); break;
                        case 3: elipse.Fill = new SolidColorBrush(Colors.Brown); break;
                        case 4: elipse.Fill = new SolidColorBrush(Colors.Red); break;
                        case 5: elipse.Fill = new SolidColorBrush(Colors.Pink); break;
                        case 6: elipse.Fill = new SolidColorBrush(Colors.Green); break;
                        case 7: elipse.Fill = new SolidColorBrush(Colors.Black); break;
                        //case 8: elipse.Fill = new SolidColorBrush(Colors.Magenta); break;
                        //case 9: elipse.Fill = new SolidColorBrush(Colors.Blue); break;
                        //case 10: elipse.Fill = new SolidColorBrush(Colors.BlueViolet); break;
                    }
                    elipse.MouseEnter += elipse_MouseEnter;
                    elipse.MouseLeave += elipse_MouseLeave;
                    elipse.MouseLeftButtonDown += elipse_MouseLeftButtonDown;
                    elipse.Cursor = Cursors.Hand;
                    elipse.Tag = id;
                    Canvas.SetLeft(elipse, x);
                    Canvas.SetTop(elipse, y-w);
                    CanvasContenedor.Children.Add(elipse);
                    x += w;
                    circulos.Add(new Circulo() { x = j, y = i, nrocolor = n, id = id, elipse = elipse, left = x-w, top = y - w });
                    id++;
                }
                y -= w;
                x = 0;// (int)CanvasContenedor.Width;
            }
            nrocirculos = circulos.Count();
        }

        void Buscar(int i)
        {
            
            //for (int i = 0; i < circulos.Count();i++ )
            //{
                circulosactivados = new List<Circulo>();
                var cc = circulos.Where(p => p.nrocolor == circulos[i].nrocolor && p.id != circulos[i].id).Select(p => p).ToList();
                circulosactivados.Add(circulos[i]);
                BuscarCirculos(circulos[i], cc);
                if (circulosactivados.Count() > 1)
                {
                    foreach (var c in circulosactivados)
                    {
                        var circulo = circulos.Where(p => p.id == c.id).Select(p => p).First();
                        circulo.vacio = true;
                        circulos.Remove(circulo);
                        CanvasContenedor.Children.Remove(c.elipse);
                    }
                    var _x = circulosactivados.Select(p => p.x).Distinct().ToList();
                    int y = (int)CanvasContenedor.Height;
                    int w = 0;
                    if (columnas >= filas)
                        w = (int)CanvasContenedor.Width / columnas;
                    else
                        w = (int)CanvasContenedor.Height / filas;
                    foreach (var x in _x)
                    {
                        var elementos = circulos.Where(p => p.x == x).Select(p => p).ToList();
                        if (elementos.Count() > 0)
                        {
                            for (int j = 0; j < elementos.Count(); j++)
                            {
                                CanvasContenedor.Children.Remove(elementos[j].elipse);
                                Canvas.SetLeft(elementos[j].elipse, elementos[j].left);
                                Canvas.SetTop(elementos[j].elipse, y - w);
                                elementos[j].y = j;
                                elementos[j].top = y - w;
                                CanvasContenedor.Children.Add(elementos[j].elipse);
                                y -= w;
                            }
                            y = (int)CanvasContenedor.Height;
                        }
                    }
                    for (int k = 0; k < columnas; k++)//verificar columnas vacias
                    {
                        int multiplicador = 1;
                        var elementos = circulos.Where(p => p.x == k).Select(p => p.x).ToList();
                        if (elementos.Count() == 0)
                        {
                            for (int m = k + 1; m < columnas; m++)
                            {
                                var _elementos = circulos.Where(p => p.x == m).Select(p => p).ToList();
                                if (_elementos.Count() > 0)
                                {
                                    for (int j = 0; j < _elementos.Count(); j++)
                                    {
                                        CanvasContenedor.Children.Remove(_elementos[j].elipse);
                                        Canvas.SetLeft(_elementos[j].elipse, _elementos[j].left - (w * multiplicador));
                                        Canvas.SetTop(_elementos[j].elipse, _elementos[j].top);
                                        _elementos[j].x = m - 1;
                                        _elementos[j].left = _elementos[j].left - w;
                                        CanvasContenedor.Children.Add(_elementos[j].elipse);
                                    }
                                }
                                else
                                    multiplicador += 1;
                            }
                        }
                    }
                    nroi = 0;
                    //i = circulos.Count();
                }
                else
                {
                    circulosactivados.Clear(); nroi += 1;
                }
            //}
        }

        void elipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (circulosactivados.Count() > 1)
            {
                foreach (var c in circulosactivados)
                {                    
                    var circulo = circulos.Where(p => p.id == c.id).Select(p => p).First();
                    circulo.vacio = true;
                    circulos.Remove(circulo);
                    CanvasContenedor.Children.Remove(c.elipse);
                }
                var _x = circulosactivados.Select(p => p.x).Distinct().ToList();//buscar solo columnas
                int y = (int)CanvasContenedor.Height;
                int w = 0;
                if (columnas >= filas)
                    w = (int)CanvasContenedor.Width / columnas;
                else
                    w = (int)CanvasContenedor.Height / filas;
                foreach (var x in _x)
                {
                    var elementos = circulos.Where(p => p.x == x).Select(p => p).ToList();
                    if (elementos.Count() > 0)
                    {
                        for (int i = 0; i < elementos.Count(); i++)
                        {
                            CanvasContenedor.Children.Remove(elementos[i].elipse);
                            elementos[i].y = i;
                            Canvas.SetLeft(elementos[i].elipse, elementos[i].left);
                            Canvas.SetTop(elementos[i].elipse, y - w);
                            elementos[i].top = y - w;
                            CanvasContenedor.Children.Add(elementos[i].elipse);
                            y -= w;
                        }
                        y = (int)CanvasContenedor.Height;
                    }
                }
                for (int i = 0; i < columnas;i++ )//verificar columnas vacias
                {
                    int multiplicador = 1;
                    var elementos = circulos.Where(p => p.x == i).Select(p => p.x).ToList();
                    if (elementos.Count() == 0)
                    {
                        for (int m = i + 1; m < columnas;m++ )
                        {
                            var _elementos = circulos.Where(p => p.x == m).Select(p => p).ToList();
                            if (_elementos.Count() > 0)
                            {
                                for (int j = 0; j < _elementos.Count(); j++)
                                {
                                    CanvasContenedor.Children.Remove(_elementos[j].elipse);
                                    Canvas.SetLeft(_elementos[j].elipse, _elementos[j].left - (w * multiplicador));
                                    Canvas.SetTop(_elementos[j].elipse, _elementos[j].top);
                                    _elementos[j].x = m - 1;
                                    _elementos[j].left = _elementos[j].left - w;
                                    CanvasContenedor.Children.Add(_elementos[j].elipse);
                                }
                            }
                            else
                                multiplicador += 1;
                        }
                    }
                }
                circulosactivados.Clear();
            }
            else
                MessageBox.Show("Minimo de circulos iguales : 3");
            circulosactivados.Clear();
        }

        void elipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse elipse = sender as Ellipse;
            elipse.Opacity = 0.5;
            foreach (var c in circulosactivados)
                c.elipse.Opacity = 0.5;
            circulosactivados.Clear();
        }
        List<Circulo> circulosactivados;

        void elipse_MouseEnter(object sender, MouseEventArgs e)
        {
            circulosactivados = new List<Circulo>();
            Ellipse elipse = sender as Ellipse;
            elipse.Opacity = 1;
            var c = circulos.Where(p => p.id == (int)elipse.Tag).Select(p => p).First();
            var cc = circulos.Where(p => p.nrocolor == c.nrocolor && p.id != c.id).Select(p => p).ToList();
            circulosactivados.Add(c);
            BuscarCirculos(c, cc);
        }
        
        void BuscarCirculos(Circulo c,List<Circulo> circulos)
        {
            foreach (var _c in circulos)
            {
                if (_c.x == c.x && _c.y == c.y - 1)//abajo
                {
                    var existe = circulosactivados.Where(p => p.id == _c.id).FirstOrDefault();
                    if (existe == null)
                    {
                        _c.elipse.Opacity = 1;
                        circulosactivados.Add(_c);
                        var cc = circulos.Where(p => p.id != _c.id).Select(p => p).ToList();
                        BuscarCirculos(_c, cc);
                    }
                }
                if (_c.x == c.x && _c.y == c.y + 1)//arriba
                {
                    var existe = circulosactivados.Where(p => p.id == _c.id).FirstOrDefault();
                    if (existe == null)
                    {
                        _c.elipse.Opacity = 1;
                        circulosactivados.Add(_c);
                        var cc = circulos.Where(p => p.id != _c.id).Select(p => p).ToList();
                        BuscarCirculos(_c, cc);
                    }
                }
                if (_c.x == c.x - 1 && _c.y == c.y)//izquierda
                {
                    var existe = circulosactivados.Where(p => p.id == _c.id).FirstOrDefault();
                    if (existe == null)
                    {
                        _c.elipse.Opacity = 1;
                        circulosactivados.Add(_c);
                        var cc = circulos.Where(p => p.id != _c.id).Select(p => p).ToList();
                        BuscarCirculos(_c, cc);
                    }
                }
                if (_c.x == c.x + 1 && _c.y == c.y)//derecha
                {
                    var existe = circulosactivados.Where(p => p.id == _c.id).FirstOrDefault();
                    if (existe == null)
                    {
                        _c.elipse.Opacity = 1;
                        circulosactivados.Add(_c);
                        var cc = circulos.Where(p => p.id != _c.id).Select(p => p).ToList();
                        BuscarCirculos(_c, cc);
                    }
                }
            }
        }
        public class Circulo
        {
            public int id { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int nrocolor { get; set; }
            public Ellipse elipse { get; set; }
            public bool vacio { get; set; }
            public int left { get; set; }
            public int top { get; set; }
        }
        int nroi = 0;
        int nrocirculos = 0;

        private void btnBuscarSolucion_Click(object sender, RoutedEventArgs e)
        {
            while (nrocirculos == circulos.Count())
            {
                if (nroi < circulos.Count())
                    Buscar(nroi);
                else
                    nrocirculos = -1;
            }                        
            if (nrocirculos == -1)
                MessageBox.Show("No hay mas soluciones");
            else
                if (circulos.Count() == 0)
                    MessageBox.Show(":)");
                else
                    nrocirculos = circulos.Count();
            //while(nroi < circulos.Count())
                //Buscar(nroi);
            //nroi = 0;
            //while (nroi < circulos.Count())
            //    Buscar(nroi);  
        }        
    }
}
