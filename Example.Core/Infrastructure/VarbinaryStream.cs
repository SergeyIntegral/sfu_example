using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Example.Core.Infrastructure
{
    public class VarbinaryStream : Stream
    {
        private readonly SqlConnection _connection;
        private readonly string _tableName;
        private readonly string _binaryColumn;
        private readonly string _keyColumn;
        private readonly int _keyValue;

        private long _offset;

        //private readonly SqlDataReader _sqlReader;
        //private long _sqlReadPosition;

        public VarbinaryStream(
            SqlConnection Connection,
            string TableName,
            string BinaryColumn,
            string KeyColumn,
            int KeyValue)
        {
            _connection = Connection;
            _tableName = TableName;
            _binaryColumn = BinaryColumn;
            _keyColumn = KeyColumn;
            _keyValue = KeyValue;
        }

        // this method will be called as part of the Stream ímplementation when we try to write to our VarbinaryStream class.
        public override void Write(byte[] buffer, int index, int count)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                if (_offset == 0)
                {
                    // for the first write we just send the bytes to the Column
                    SqlCommand cmd = new SqlCommand(
                                                @"UPDATE [dbo].[" + _tableName + @"]
                                                    SET [" + _binaryColumn + @"] = @firstchunk 
                                                WHERE [" + _keyColumn + "] = @id",
                                            _connection);

                    cmd.Parameters.Add(new SqlParameter("@firstchunk", buffer));
                    cmd.Parameters.Add(new SqlParameter("@id", _keyValue));

                    cmd.ExecuteNonQuery();

                    _offset = count;
                }
                else
                {
                    // for all updates after the first one we use the TSQL command .WRITE() to append the data in the database
                    SqlCommand cmd = new SqlCommand(
                                            @"UPDATE [dbo].[" + _tableName + @"]
                                                SET [" + _binaryColumn + @"].WRITE(@chunk, NULL, @length)
                                            WHERE [" + _keyColumn + "] = @id",
                                         _connection);

                    cmd.Parameters.Add(new SqlParameter("@chunk", buffer));
                    cmd.Parameters.Add(new SqlParameter("@length", count));
                    cmd.Parameters.Add(new SqlParameter("@id", _keyValue));

                    cmd.ExecuteNonQuery();

                    _offset += count;
                }
            }
            catch (Exception e)
            {
                // log errors here
            }
        }

        // this method will be called as part of the Stream ímplementation when we try to read from our VarbinaryStream class.
        public override int Read(byte[] buffer, int offset, int count)
        {
            //try
            //{
            //    long bytesRead = _sqlReader.GetBytes(0, _sqlReadPosition, buffer, offset, count);
            //    _sqlReadPosition += bytesRead;
            //    return (int)bytesRead;
            //}
            //catch (Exception e)
            //{
            //    // log errors here
            //}
            return -1;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Position
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value;
            }
        }

        #region unimplemented methods
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        #endregion unimplemented methods
    }
}