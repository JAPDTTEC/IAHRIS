using System;
using global::System.Data;
using global::System.Data.OleDb;
using Microsoft.VisualBasic;

namespace IAHRIS.BBDD
{
    /// <summary>
    /// Encapsulación para el acceso a una base de datos OleDb
    /// </summary>
    /// <remarks></remarks>
    public class OleDbDataBase
    {

        /// <summary>
        /// ConnString para la base de datos deseada
        /// </summary>
        /// <remarks></remarks>
        private string _ConnString;

        /// <summary>
        /// Conexión privada para comenzar el desarrollo del pooling
        /// </summary>
        /// <remarks></remarks>
        private OleDbConnection _myConn;

        /// <summary>
        /// Transacción local para cuando sea necesario
        /// </summary>
        /// <remarks></remarks>
        private OleDbTransaction _transaccion;
        private string _rutaMDB;

        public string ruta
        {
            get
            {
                return _rutaMDB;
            }

            set
            {
                _rutaMDB = value;
            }
        }

        /// <summary>
        /// Conecta a la base de datos
        /// </summary>
        /// <remarks></remarks>
        private void Conectar()
        {
            try
            {
                if (_myConn is null)
                {
                    // Crear una conexión nueva
                    _myConn = GetNuevaConexion();
                }
                else if (_myConn.State != ConnectionState.Open)
                {
                    // Crear una conexión nueva
                    _myConn = GetNuevaConexion();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DATABASE.CONECTAR", ex);
            }
        }

        private OleDbConnection GetNuevaConexion()
        {
            OleDbConnection conn;
            string connectionString;
            try
            {
                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _rutaMDB;
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Imposible crear la conexión a la base de datos", ex);
            }

            return conn;
        }

        /// <summary>
        /// Desconecta de la base de datos
        /// </summary>
        /// <remarks></remarks>
        public void Desconectar()
        {
            _myConn.Close();
            while (_myConn.State != ConnectionState.Closed)
            {
            }
            // Me._myConn = Nothing
        }

        /// <summary>
        /// Obtiene una tabla completa de la base de datos en memoria
        /// </summary>
        /// <param name="NombreTabla">Nombre de la tabla</param>
        /// <returns>Objeto DataTable con los datos</returns>
        /// <remarks></remarks>
        public DataTable GetTablaCompleta(string NombreTabla)
        {
            string sql;
            sql = "Select * From " + NombreTabla;
            var odbda = new OleDbDataAdapter(sql, _myConn);
            var cb = new OleDbCommandBuilder(odbda);
            var dt = new DataTable();
            odbda.Fill(dt);
            odbda.Dispose();
            cb.Dispose();
            return dt;
        }

        /// <summary>
        /// Obtiene un datatable con el resultado de una SQL tipo Select en memoria
        /// </summary>
        /// <param name="SQL">Sentencia SQL correcta</param>
        /// <returns>Objeto DataTable con los datos</returns>
        /// <remarks></remarks>
        public DataTable GetTablaSQL(string SQL)
        {
            var odbda = new OleDbDataAdapter(SQL, _myConn);
            var cb = new OleDbCommandBuilder(odbda);
            var dt = new DataTable();
            odbda.Fill(dt);
            odbda.Dispose();
            cb.Dispose();
            return dt;
        }

        /// <summary>
        /// Obtiene los resultados de una consulta SQL
        /// </summary>
        /// <param name="SQL">SQL</param>
        /// <returns>OleDbDataReader con el resultado de la consulta</returns>
        /// <remarks></remarks>
        public OleDbDataReader GetReader(string SQL)
        {
            var command = new OleDbCommand(SQL, _myConn);
            OleDbDataReader reader;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
                reader = null;
            }

            command.Dispose();
            return reader;
        }

        /// <summary>
        /// Obtiene un dataset con el resultado de una SQL tipo Select en memoria
        /// </summary>
        /// <param name="SQL">Sentencia SQL correcta</param>
        /// <returns>Objeto DataSet con los datos</returns>
        /// <remarks></remarks>
        public DataSet RellenarDataSet(string dsName, string SQL)
        {

            // Create a SqlDataAdapter for the Suppliers table.
            var adapter = new OleDbDataAdapter();

            // A table mapping names the DataTable.
            adapter.TableMappings.Add("Table", "Suppliers");

            // Create a SqlCommand to retrieve Suppliers data.
            var command = new OleDbCommand(SQL, _myConn, _transaccion);
            command.CommandType = CommandType.Text;

            // Set the SqlDataAdapter's SelectCommand.
            adapter.SelectCommand = command;

            // Fill the DataSet.
            var dataSet = new DataSet(dsName);
            adapter.Fill(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Inicia una transacción en la base de datos
        /// </summary>
        public void ComenzarTransaccion()
        {
            if (_transaccion is object)
                _transaccion.Dispose();
            _transaccion = _myConn.BeginTransaction();
        }

        /// <summary>
        /// Realiza/retracta la transacción actual
        /// </summary>
        /// <param name="commit"></param>
        /// <remarks></remarks>
        public void TerminarTransaccion(bool commit)
        {
            if (_transaccion is object)
            {
                if (commit)
                {
                    _transaccion.Commit();
                }
                else
                {
                    _transaccion.Rollback();
                }

                _transaccion.Dispose();
            }

            _transaccion = null;
        }

        /// <summary>
        /// Ejecuta una SQL tipo Insert, Delete o Update
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns>Número de filas afectadas por la consulta</returns>
        /// <remarks></remarks>
        public int EjecutarSQL(string SQL)
        {
            int NumRows;
            try
            {
                // rellenamos el dataset con los datos actuales de la tabla
                var command = new OleDbCommand(SQL, _myConn, _transaccion);
                NumRows = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString() + Constants.vbCrLf + SQL, MsgBoxStyle.Critical);
                NumRows = -1;
            } // Código de error

            return NumRows;
        }

        public bool InsertarRegistro(string NombreTabla, string[] Campos, string[] Valores)
        {
            string SQL = "Select * From [" + NombreTabla + "]";
            DataSet Ds;
            int NumRows;
            try
            {
                // rellenamos el dataset con los datos actuales de la tabla
                var command = new OleDbCommand(SQL, _myConn, _transaccion);
                var da = new OleDbDataAdapter(command);
                var cmb = new OleDbCommandBuilder(da);
                da.InsertCommand = cmb.GetInsertCommand();
                Ds = new DataSet();
                da.Fill(Ds);

                // insertamos una nueva fila
                var newRow = Ds.Tables[0].NewRow();
                for (int i = 0, loopTo = Campos.Length - 1; i <= loopTo; i++)
                    newRow[Campos[i]] = Valores[i];
                Ds.Tables[0].Rows.Add(newRow);

                // actualizamos la base de datos con la fila o filas insertadas
                NumRows = da.Update(Ds.GetChanges(DataRowState.Added));
            }
            catch (Exception ex)
            {
                // MsgBox("InsertarRegistro:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
                return false;
            }

            return NumRows == 1;
        }

        /// <summary>
        /// Inserta N registros en la tabla deseada de una sola vez
        /// </summary>
        /// <param name="NombreTabla"></param>
        /// <param name="Campos"></param>
        /// <param name="Valores"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool InsertarRegistros(string NombreTabla, string[] Campos, string[][] Valores)
        {
            string SQL = "Select * From [" + NombreTabla + "]";
            DataSet Ds;
            int NumRows;
            try
            {
                // rellenamos el dataset con los datos actuales de la tabla
                var command = new OleDbCommand(SQL, _myConn, _transaccion);
                var da = new OleDbDataAdapter(command);
                var cmb = new OleDbCommandBuilder(da);
                da.InsertCommand = cmb.GetInsertCommand();
                Ds = new DataSet();
                da.Fill(Ds);
                for (int j = 0, loopTo = Information.UBound(Valores); j <= loopTo; j++)
                {
                    // insertamos una nueva fila
                    var newRow = Ds.Tables[0].NewRow();
                    for (int i = 0, loopTo1 = Campos.Length - 1; i <= loopTo1; i++)
                        newRow[Campos[i]] = Valores[j][i];
                    Ds.Tables[0].Rows.Add(newRow);
                }

                // actualizamos la base de datos con la fila o filas insertadas
                NumRows = da.Update(Ds.GetChanges(DataRowState.Added));
            }
            catch (Exception ex)
            {
                // MsgBox("InsertarRegistro:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
                return false;
            }

            return NumRows != 0;
        }

        ~OleDbDataBase()
        {
            // INFO: el método Finalize se llama automáticamente al perder el objeto todas las referencias que posea dentro del programa

            // --------------------------------------------------------
            // Liberar las conexiones a la base de datos
            // --------------------------------------------------------
            if (_myConn is object)
            {
                if (_myConn.State != ConnectionState.Closed)
                {
                    try
                    {
                        _myConn.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }

                _myConn.Dispose();
                _myConn = null;
            }
        }

        /// <summary>
        /// Constructor: conecta a la base de datos especificada
        /// </summary>
        /// <param name="ConnectionString">Cadena de conexión</param>
        /// <remarks></remarks>
        public OleDbDataBase(string ConnectionString, string ruta)
        {
            _ConnString = ConnectionString;
            _rutaMDB = ruta;
            try
            {
                Conectar();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.ToString(), MsgBoxStyle.Critical);
                
                //Finalize(); //CHECK: C# no permite esto. Es posible que haya que encontrar una forma de cerrar.
            }
        }
    }
}