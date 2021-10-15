Imports System.Data
Imports System.Data.OleDb

Imports System.ComponentModel

Namespace BBDD
    ''' <summary>
    ''' Encapsulación para el acceso a una base de datos OleDb
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OleDbDataBase

        ''' <summary>
        ''' ConnString para la base de datos deseada
        ''' </summary>
        ''' <remarks></remarks>
        Private _ConnString As String

        ''' <summary>
        ''' Conexión privada para comenzar el desarrollo del pooling
        ''' </summary>
        ''' <remarks></remarks>
        Private _myConn As OleDbConnection

        ''' <summary>
        ''' Transacción local para cuando sea necesario
        ''' </summary>
        ''' <remarks></remarks>
        Private _transaccion As OleDbTransaction

        Private _rutaMDB As String

        Public Property ruta() As String
            Get
                Return _rutaMDB
            End Get
            Set(ByVal value As String)
                Me._rutaMDB = value
            End Set
        End Property

        ''' <summary>
        ''' Conecta a la base de datos
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub Conectar()
            Try
                If _myConn Is Nothing Then
                    ' Crear una conexión nueva
                    _myConn = Me.GetNuevaConexion()
                Else
                    If _myConn.State <> ConnectionState.Open Then
                        ' Crear una conexión nueva
                        _myConn = Me.GetNuevaConexion()
                    End If
                End If

            Catch ex As Exception
                Throw New Exception("DATABASE.CONECTAR", ex)
            End Try
        End Sub

        Private Function GetNuevaConexion() As OleDbConnection
            Dim conn As OleDbConnection
            Dim connectionString As String
            Try
                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & _rutaMDB
                conn = New OleDbConnection(connectionString)
                conn.Open()
            Catch ex As Exception
                Throw New Exception("Imposible crear la conexión a la base de datos", ex)
            End Try

            Return conn

        End Function

        ''' <summary>
        ''' Desconecta de la base de datos
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Desconectar()
            Me._myConn.Close()
            While Me._myConn.State <> ConnectionState.Closed

            End While
            'Me._myConn = Nothing
        End Sub

        ''' <summary>
        ''' Obtiene una tabla completa de la base de datos en memoria
        ''' </summary>
        ''' <param name="NombreTabla">Nombre de la tabla</param>
        ''' <returns>Objeto DataTable con los datos</returns>
        ''' <remarks></remarks>
        Public Function GetTablaCompleta(ByVal NombreTabla As String) As DataTable
            Dim sql As String
            sql = "Select * From " & NombreTabla
            Dim odbda As New OleDbDataAdapter(sql, Me._myConn)
            Dim cb As New OleDbCommandBuilder(odbda)
            Dim dt As DataTable = New DataTable
            odbda.Fill(dt)
            odbda.Dispose()
            cb.Dispose()
            Return dt
        End Function

        ''' <summary>
        ''' Obtiene un datatable con el resultado de una SQL tipo Select en memoria
        ''' </summary>
        ''' <param name="SQL">Sentencia SQL correcta</param>
        ''' <returns>Objeto DataTable con los datos</returns>
        ''' <remarks></remarks>
        Public Function GetTablaSQL(ByVal SQL As String) As DataTable
            Dim odbda As New OleDbDataAdapter(SQL, Me._myConn)
            Dim cb As New OleDbCommandBuilder(odbda)
            Dim dt As DataTable = New DataTable
            odbda.Fill(dt)
            odbda.Dispose()
            cb.Dispose()
            Return dt
        End Function

        ''' <summary>
        ''' Obtiene los resultados de una consulta SQL
        ''' </summary>
        ''' <param name="SQL">SQL</param>
        ''' <returns>OleDbDataReader con el resultado de la consulta</returns>
        ''' <remarks></remarks>
        Public Function GetReader(ByVal SQL As String) As OleDbDataReader
            Dim command As New OleDbCommand(SQL, Me._myConn)
            Dim reader As OleDbDataReader
            Try
                reader = command.ExecuteReader()
            Catch ex As Exception
                MsgBox(ex.Message)
                reader = Nothing
            End Try
            command.Dispose()
            Return reader
        End Function

        ''' <summary>
        ''' Obtiene un dataset con el resultado de una SQL tipo Select en memoria
        ''' </summary>
        ''' <param name="SQL">Sentencia SQL correcta</param>
        ''' <returns>Objeto DataSet con los datos</returns>
        ''' <remarks></remarks>
        Public Function RellenarDataSet(ByVal dsName As String, ByVal SQL As String) As DataSet

            ' Create a SqlDataAdapter for the Suppliers table.
            Dim adapter As OleDbDataAdapter = New OleDbDataAdapter()

            ' A table mapping names the DataTable.
            adapter.TableMappings.Add("Table", "Suppliers")

            ' Create a SqlCommand to retrieve Suppliers data.
            Dim command As OleDbCommand = New OleDbCommand(SQL, Me._myConn, Me._transaccion)
            command.CommandType = CommandType.Text

            ' Set the SqlDataAdapter's SelectCommand.
            adapter.SelectCommand = command

            ' Fill the DataSet.
            Dim dataSet As DataSet = New DataSet(dsName)
            adapter.Fill(dataSet)

            Return dataSet

        End Function

        ''' <summary>
        ''' Inicia una transacción en la base de datos
        ''' </summary>
        Public Sub ComenzarTransaccion()
            If Not Me._transaccion Is Nothing Then Me._transaccion.Dispose()

            Me._transaccion = Me._myConn.BeginTransaction()
        End Sub

        ''' <summary>
        ''' Realiza/retracta la transacción actual
        ''' </summary>
        ''' <param name="commit"></param>
        ''' <remarks></remarks>
        Public Sub TerminarTransaccion(ByVal commit As Boolean)
            If Not Me._transaccion Is Nothing Then
                If commit Then
                    Me._transaccion.Commit()
                Else
                    Me._transaccion.Rollback()
                End If
                Me._transaccion.Dispose()
            End If
            Me._transaccion = Nothing
        End Sub

        ''' <summary>
        ''' Ejecuta una SQL tipo Insert, Delete o Update
        ''' </summary>
        ''' <param name="SQL"></param>
        ''' <returns>Número de filas afectadas por la consulta</returns>
        ''' <remarks></remarks>
        Public Function EjecutarSQL(ByVal SQL As String) As Integer
            Dim NumRows As Integer
            Try
                'rellenamos el dataset con los datos actuales de la tabla
                Dim command As New OleDbCommand(SQL, Me._myConn, Me._transaccion)
                NumRows = command.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString & vbCrLf & SQL, MsgBoxStyle.Critical)
                NumRows = -1 ' Código de error
            End Try

            Return NumRows
        End Function

        Public Function InsertarRegistro(ByVal NombreTabla As String, ByVal Campos() As String, ByVal Valores() As String) As Boolean
            Dim SQL As String = "Select * From [" & NombreTabla & "]"
            Dim Ds As DataSet

            Dim NumRows As Integer

            Try
                'rellenamos el dataset con los datos actuales de la tabla
                Dim command As New OleDbCommand(SQL, Me._myConn, Me._transaccion)
                Dim da As New OleDbDataAdapter(command)
                Dim cmb As New OleDbCommandBuilder(da)
                da.InsertCommand = cmb.GetInsertCommand()
                Ds = New DataSet

                da.Fill(Ds)

                'insertamos una nueva fila
                Dim newRow As DataRow = Ds.Tables(0).NewRow()
                For i As Integer = 0 To Campos.Length - 1
                    newRow.Item(Campos(i)) = Valores(i)
                Next

                Ds.Tables(0).Rows.Add(newRow)

                'actualizamos la base de datos con la fila o filas insertadas
                NumRows = da.Update(Ds.GetChanges(DataRowState.Added))
            Catch ex As Exception
                'MsgBox("InsertarRegistro:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
                Return False
            End Try
            Return (NumRows = 1)

        End Function

        ''' <summary>
        ''' Inserta N registros en la tabla deseada de una sola vez
        ''' </summary>
        ''' <param name="NombreTabla"></param>
        ''' <param name="Campos"></param>
        ''' <param name="Valores"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsertarRegistros(ByVal NombreTabla As String, ByVal Campos() As String, ByVal Valores()() As String) As Boolean
            Dim SQL As String = "Select * From [" & NombreTabla & "]"
            Dim Ds As DataSet

            Dim NumRows As Integer

            Try
                'rellenamos el dataset con los datos actuales de la tabla
                Dim command As New OleDbCommand(SQL, Me._myConn, Me._transaccion)
                Dim da As New OleDbDataAdapter(command)
                Dim cmb As New OleDbCommandBuilder(da)
                da.InsertCommand = cmb.GetInsertCommand()
                Ds = New DataSet

                da.Fill(Ds)

                For j As Integer = 0 To UBound(Valores)
                    'insertamos una nueva fila
                    Dim newRow As DataRow = Ds.Tables(0).NewRow()
                    For i As Integer = 0 To Campos.Length - 1
                        newRow.Item(Campos(i)) = Valores(j)(i)
                    Next
                    Ds.Tables(0).Rows.Add(newRow)
                Next

                'actualizamos la base de datos con la fila o filas insertadas
                NumRows = da.Update(Ds.GetChanges(DataRowState.Added))
            Catch ex As Exception
                'MsgBox("InsertarRegistro:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
                Return False
            End Try
            Return (NumRows <> 0)

        End Function

        Protected Overrides Sub Finalize()
            ' INFO: el método Finalize se llama automáticamente al perder el objeto todas las referencias que posea dentro del programa

            '--------------------------------------------------------
            ' Liberar las conexiones a la base de datos
            '--------------------------------------------------------
            If Not _myConn Is Nothing Then
                If _myConn.State <> ConnectionState.Closed Then
                    Try
                        _myConn.Close()
                    Catch ex As Exception

                    End Try
                End If
                _myConn.Dispose()
                _myConn = Nothing
            End If

            MyBase.Finalize()
        End Sub

        ''' <summary>
        ''' Constructor: conecta a la base de datos especificada
        ''' </summary>
        ''' <param name="ConnectionString">Cadena de conexión</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ConnectionString As String, ByVal ruta As String)
            Me._ConnString = ConnectionString
            Me._rutaMDB = ruta

            Try
                Me.Conectar()
            Catch ex As Exception
                MsgBox(ex.ToString(), MsgBoxStyle.Critical)
                Me.Finalize()
            End Try
        End Sub

    End Class

End Namespace