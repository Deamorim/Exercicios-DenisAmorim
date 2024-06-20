using System;
using System.Drawing;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {
        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            Numero = numero;
            Titular = titular;
            Deposito(depositoInicial);
        }

        public int Numero { get; }
        public string Titular { get; private set; }

        private static double TaxaSaque = 3.50;

        private double _saldo = 0;
        public double Saldo
        {
            get
            {
                return _saldo;
            }
            set
            {
                if (value < 0)
                {
                    return;
                }

                _saldo = value;
            }
        }

        public void Saque(double valor)
        {
            _saldo -= valor + TaxaSaque;
        }

        public void Deposito(double valor)
        {
            _saldo += valor;
        }

        public void AlterarTitular(string novoTitular)
        {
            Titular = novoTitular;
        }
    }
}
