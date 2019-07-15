using tabuleiro;
using System.Text;
using System;
using xadrez;
using System.Collections.Generic;

namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirTabuleiro ( Tabuleiro tab )
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i);
                Console.Write(" ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    ImprimirPeca(tab.Peca(i, j));   
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirTabuleiro ( Tabuleiro tab, bool[,] posicoesPosiveis )
        {
            ConsoleColor fundoOri = Console.BackgroundColor;
            ConsoleColor fundoAlt = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i);
                Console.Write(" ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (posicoesPosiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlt;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOri;
                    }
                    ImprimirPeca(tab.Peca(i, j));
                    Console.BackgroundColor = fundoOri;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOri;
        }

        public static PosicaoXadrez LerPosicaoXadrez ()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPeca ( Peca peca )
        {
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.Cor == Cor.Branca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
            
        }

        public static void ImprimirPartida ( PartidaDeXadrez partida )
        {
            ImprimirTabuleiro(partida._tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine("\nTurno: {0}\nAguardando jogada: {1}", partida._turno, partida._jogadorAtual);
        }

        private static void ImprimirPecasCapturadas ( PartidaDeXadrez partida )
        {
            Console.WriteLine("Peças capturadas: ");
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
        }

        private static void ImprimirConjunto ( HashSet<Peca> hashSet )
        {
            Console.Write("[");
            foreach (Peca x in hashSet)
            {
                Console.Write(x + " ");
            }
            Console.Write("]\n");
        }
    }
}
