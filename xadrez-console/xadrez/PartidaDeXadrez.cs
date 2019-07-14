using System;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro _tab { get; private set; }
        public int _turno { get; private set; }
        public Cor _jogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaDeXadrez ()
        {
            _tab = new Tabuleiro(8, 8);
            _turno = 1;
            _jogadorAtual = Cor.Branca;
            Terminada = false;
            ColocarPecas();
        }

        private void ColocarPecas ()
        {
            _tab.ColocarPeca(new Torre(Cor.Branca, _tab), new PosicaoXadrez('c', 1).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Branca, _tab), new PosicaoXadrez('c', 2).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Branca, _tab), new PosicaoXadrez('d', 2).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Branca, _tab), new PosicaoXadrez('e', 2).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Branca, _tab), new PosicaoXadrez('e', 1).ToPosicao());
            _tab.ColocarPeca(new Rei(Cor.Branca, _tab), new PosicaoXadrez('d', 1).ToPosicao());

            _tab.ColocarPeca(new Torre(Cor.Preta, _tab), new PosicaoXadrez('c', 8).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Preta, _tab), new PosicaoXadrez('c', 7).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Preta, _tab), new PosicaoXadrez('d', 7).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Preta, _tab), new PosicaoXadrez('e', 7).ToPosicao());
            _tab.ColocarPeca(new Torre(Cor.Preta, _tab), new PosicaoXadrez('e', 8).ToPosicao());
            _tab.ColocarPeca(new Rei(Cor.Preta, _tab), new PosicaoXadrez('d', 8).ToPosicao());
        }

        public void ExecutaMovimento ( Posicao origem, Posicao destino )
        {
            Peca p = _tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = _tab.RetirarPeca(destino);
            _tab.ColocarPeca(p, destino);
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
