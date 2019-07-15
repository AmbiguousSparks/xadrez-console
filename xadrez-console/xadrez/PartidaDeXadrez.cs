using System;
using tabuleiro;
using System.Collections.Generic;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro _tab { get; private set; }
        public int _turno { get; private set; }
        public Cor _jogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;

        public PartidaDeXadrez ()
        {
            _tab = new Tabuleiro(8, 8);
            _turno = 1;
            _jogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ColocarNovaPeca (char coluna, int linha, Peca p)
        {
            _tab.ColocarPeca(p, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(p);
        }

        private void ColocarPecas ()
        {
            ColocarNovaPeca('c', 1, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('c', 2, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('d', 2, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('e', 2, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('e', 1, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('d', 1, new Rei(Cor.Branca, _tab));

            ColocarNovaPeca('c', 8, new Torre(Cor.Preta, _tab));
            ColocarNovaPeca('c', 7, new Torre(Cor.Preta, _tab));
            ColocarNovaPeca('d', 7, new Torre(Cor.Preta, _tab));
            ColocarNovaPeca('e', 8, new Torre(Cor.Preta, _tab));
            ColocarNovaPeca('e', 7, new Torre(Cor.Preta, _tab));
            ColocarNovaPeca('d', 8, new Rei(Cor.Preta, _tab));
        }

        public void ExecutaMovimento ( Posicao origem, Posicao destino )
        {
            Peca p = _tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = _tab.RetirarPeca(destino);
            _tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }
        }

        public HashSet<Peca> PecasCapturadas ( Cor cor )
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo ( Cor cor )
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public void RealizaJogada ( Posicao origem, Posicao destino )
        {
            ExecutaMovimento(origem, destino);
            _turno++;
            MudaJogador();
        }

        public void ValidarPosicaoOrigem ( Posicao origem )
        {
            if (_tab.Peca(origem) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (_jogadorAtual != _tab.Peca(origem).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!_tab.Peca(origem).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDestino ( Posicao origem, Posicao destino )
        {
            if (!_tab.Peca(origem).PodeMoverParaUmaDadaPosicao(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador ()
        {
            _jogadorAtual = (_jogadorAtual == Cor.Branca) ? Cor.Preta : Cor.Branca;
        }
    }
}
