using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main ( string[] args )
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada)
                {
                    Console.Clear();
                    Tela.ImprimirTabuleiro(partida._tab);

                    Console.WriteLine();

                    Console.WriteLine("Turno: {0}\nAguardando Jogada: {1}", partida._turno, partida._jogadorAtual);

                    try
                    {
                        Console.Write("\nOrigem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        bool[,] posicoesPosiveis = partida._tab.Peca(origem).MovimentosPossiveis();

                        Console.Clear();

                        Tela.ImprimirTabuleiro(partida._tab, posicoesPosiveis);

                        Console.Write("\nDestino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                }
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exceção inesperada {0}", e.Message);
            }
        }
    }
}
