﻿using System;
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
        public bool Xeque { get; private set; }

        public PartidaDeXadrez ()
        {
            _tab = new Tabuleiro(8, 8);
            _turno = 1;
            _jogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Xeque = false;
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public bool TesteXequeMate ( Cor cor )
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < _tab.Linhas; i++)
                {
                    for (int j = 0; j < _tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao destino = new Posicao(i, j);
                            Posicao origem = x.Posicao;
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }                
            }
            return true;
        }

        private Cor Adversaria ( Cor cor )
        {
            return (cor == Cor.Branca) ? Cor.Preta : Cor.Branca;
        }

        private Peca Rei ( Cor cor )
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque ( Cor cor )
        {
            Peca r = Rei(cor);
            if (r == null)
            {
                throw new TabuleiroException("Não tem rei da cor no tabuleiro");
            }
            foreach (Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[r.Posicao.Linha, r.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;

        }

        public void ColocarNovaPeca ( char coluna, int linha, Peca p )
        {
            _tab.ColocarPeca(p, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(p);
        }

        private void ColocarPecas ()
        {
            ColocarNovaPeca('c', 1, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('h', 7, new Torre(Cor.Branca, _tab));
            ColocarNovaPeca('d', 1, new Rei(Cor.Branca, _tab));
            
            
            ColocarNovaPeca('b', 8, new Torre(Cor.Preta, _tab));
            ColocarNovaPeca('a', 8, new Rei(Cor.Preta, _tab));
        }

        public Peca ExecutaMovimento ( Posicao origem, Posicao destino )
        {
            Peca p = _tab.RetirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = _tab.RetirarPeca(destino);
            _tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
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
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(_jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            if (EstaEmXeque(Adversaria(_jogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }
            if (TesteXequeMate(Adversaria(_jogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                _turno++;
                MudaJogador();
            }            
        }

        private void DesfazMovimento ( Posicao origem, Posicao destino, Peca pecaCapturada )
        {
            Peca p = _tab.RetirarPeca(destino);
            p.DecrementarQtdMovimentos();
            if (pecaCapturada != null)
            {
                _tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            _tab.ColocarPeca(p, origem);
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
