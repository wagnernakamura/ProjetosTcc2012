﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ConverterTabela
{
    /// <summary>
    /// 
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomePapel"></param>
        /// <returns></returns>
        public List<object> DePara(String nomePapel)
        {
            List<object> listCotacoes = new List<object>();
            DadosBE Cotacao = null;
            DataTableReader dtr = null;

            try
            {
                dtr = RetornaDados(nomePapel);

                while (dtr.Read())
                {
                    Cotacao = new DadosBE();

                    Cotacao.Id = (int)dtr["id"];
                    Cotacao.NomeReduzido = dtr["nomeReduzido"].ToString();
                    Cotacao.DataGeracao = (DateTime)dtr["data"];
                    Cotacao.PrecoAbertura = (decimal)dtr["preco"];
                    Cotacao.PrecoAberturaNormalizado = (decimal)dtr["precoNormalizado"];

                    listCotacoes.Add(Cotacao);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dtr.Dispose();
            }

            return listCotacoes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomePapel"></param>
        /// <returns></returns>
        private DataTableReader RetornaDados(String nomePapel)
        {
            SqlCommand cmdSQL = new SqlCommand();
            DataSet dsSQL = new DataSet();
            Connection oConexao = new Connection();
            DataTableReader oDataTable = null;

            cmdSQL.Connection = oConexao.RetornaConexao();
            cmdSQL.CommandType = System.Data.CommandType.StoredProcedure;
            cmdSQL.CommandText = "NOMEPROC";

            cmdSQL.Parameters.AddWithValue("@nomepapel", nomePapel);

            try
            {
                new SqlDataAdapter(cmdSQL).Fill(dsSQL);
                oDataTable = dsSQL.CreateDataReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSQL.Connection.Dispose();
                cmdSQL.Dispose();
                dsSQL.Dispose();
            }

            return oDataTable;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class DadosBE
    {
        public int Id { get; set; }
        public string NomeReduzido { get; set; }
        public DateTime DataGeracao { get; set; }
        public decimal PrecoAbertura { get; set; }
        public decimal PrecoAberturaNormalizado { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class Connection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SqlConnection RetornaConexao()
        {
            String conex = null;

            conex = ConfigurationManager.ConnectionStrings["FinanceInvest"].ConnectionString;

            SqlConnection oConexao = new SqlConnection(conex);

            return oConexao;
        }
    }
}
