using System;
using System.Threading;
using System.Collections.Generic;

class Program

{
    static int ancho = Console.WindowWidth - 1;
    static int alto = Console.WindowHeight;

    static int puntos = 0;

    static List<int> XBalas = new List<int>();
    static List<int> YBalas = new List<int>();

    static int NumAliens = 5;
    static int[] XAliens = new int[NumAliens];
    static int[] YAliens = new int[NumAliens];

    static int dirXAliens = 1;
    static int[] dirYAliens = new int[NumAliens];
    static bool[] AliensVivos = new bool[NumAliens];

    static int XNave = ancho / 2;
    static int YNave = alto - 3;
    static int dirXNave = 0; 

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;           

        IniciarAliens();

        while (true)
        {
            if (Entradas()) break;
            
           
            MoverNave();
           
      
            MoverAlien();
    
            Colision();
            Dibujar();
            MostrarScore();
            MoverBalas();

            if (VerificarVictoria())
            {
                MostrarFinal("VICTORIA");
                break;
            }

            if (VerificarDerrota())
            {
                MostrarFinal("DERROTA");
                break;    
            }

            Thread.Sleep(20); 
        }
        Console.CursorVisible = true;
    }

    static void IniciarAliens()
    {
        Random Suerte = new Random();
        
        for (int i = 0; i < NumAliens; i++)

        {
            XAliens[i] = Suerte.Next(2, ancho - 2);
            YAliens[i] = 2;

            dirXAliens = Suerte.Next(0, 2);
            if (dirXAliens == 0)
            {
                dirXAliens = -1;
            }
            else
            {
                dirXAliens = 1;
            }

            dirYAliens[i] = 1;

            AliensVivos[i] = true;
        }
    }

    static void MoverAlien()
    {
        bool reboto = false;

        for (int i = 0 ; i < NumAliens; i++)
        {
            if (!AliensVivos[i]) continue;

            int siguienteX = XAliens[i] + dirXAliens;

            if (siguienteX < 0 || siguienteX >= ancho - 1)
            {
                reboto = true;
                break;
            }
        }   
        
        if (reboto)
        {
            dirXAliens = -dirXAliens;

            for (int j = 0; j < NumAliens; j++)
            {
                if (!AliensVivos[j]) continue;

                YAliens[j] += 1;
                XAliens[j] += dirXAliens;
                    
            }
        }
        else
        {
            for (int j = 0; j < NumAliens; j++)
            {
                if (!AliensVivos[j]) continue;

                XAliens[j] += dirXAliens;
            }
        }
    }

    static void MoverNave()
    {
        XNave = (XNave + dirXNave + ancho) % ancho;
    }

    static void MoverBalas()
    {
        for (int i = XBalas.Count - 1; i >= 0; i--)
        {
            YBalas[i]--;

            if (YBalas[i] < 0)
            {
                XBalas.RemoveAt(i);
                YBalas.RemoveAt(i);
            }
        }
    }

    static void Colision()
    {
        for (int i = XBalas.Count - 1; i >= 0; i--)
        {
            for (int j = 0 ; j < NumAliens; j++)
            {
                if (!AliensVivos[j]) continue;
                
                int DistanciaX = XBalas[i] - XAliens[j];
                if (DistanciaX < 0) 
                {
                    DistanciaX = -DistanciaX;
                }

                if (DistanciaX <= 1 && YBalas[i] == YAliens[j])
                {
                    AliensVivos[j] = false;

                    puntos += 100;
                    
                    XBalas.RemoveAt(i);
                    YBalas.RemoveAt(i);

                    break;
                }
            }
        }
    }

    static void MostrarScore()
    {
        Console.SetCursorPosition(0,1);
        Console.Write($"Score: {puntos}");
    }

    static void Dibujar()
    {
        Console.Clear();
    
        for (int i = 0; i < NumAliens; i++)
        {
            if (AliensVivos[i])
            {
                Console.SetCursorPosition(XAliens[i], YAliens[i]);
                Console.Write("👾");
            }

        }
    
        Console.SetCursorPosition(XNave, YNave);
        Console.Write("🛸");

        for (int i = 0; i < XBalas.Count; i++ )
        {
            Console.SetCursorPosition(XBalas[i], YBalas[i]);
            Console.Write("💥");
        }

        Console.SetCursorPosition(0, 0);
        Console.Write("Usa A y D para moverte, usa Spacebar para disparar. Presiona ESC para salir.");

        
    }

    static bool VerificarVictoria()
    {
        if (puntos == 500)
        {
            return true;
        }
        return false;
    }

    static bool VerificarDerrota()
    {
        for (int i = 0; i < NumAliens; i++)
        {
            if (AliensVivos[i] && YAliens[i] == YNave)
            {
                return true;
            }
        }
        return false;    
    }

    static void MostrarFinal(string resultado)
    {
        Console.Clear();

        int centroX = ancho / 4;
        int centroY = alto / 2; 

        Console.SetCursorPosition(centroX, centroY);
        Console.Write($"{resultado}");

        Console.SetCursorPosition(centroX, centroY + 1);
        Console.Write($"Final Score: {puntos}");
    }

    static bool Entradas()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(true);

            if (tecla.Key == ConsoleKey.Escape) return true;

            if (tecla.Key == ConsoleKey.A) (dirXNave) = (-1);

            if (tecla.Key == ConsoleKey.D) (dirXNave) = (1);

            if (tecla.Key == ConsoleKey.Spacebar)
            {
                
                XBalas.Add(XNave);
                YBalas.Add(YNave - 1);

            }
        }
        return false;
    }
}