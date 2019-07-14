using tabuleiro;
using System.Text;
using System;

namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirTabuleiro ( Tabuleiro tab )
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tab.Linhas; i++)
            {
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (tab.Peca(i, j) == null)
                    {
                        sb.Append("- ");
                    }
                    else
                    {
                        sb.Append(tab.Peca(i, j));
                        sb.Append(" ");
                    }
                }
                sb.AppendLine("");
            }
            Console.WriteLine(sb);
        }
    }
}
