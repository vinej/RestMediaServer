using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class TodoDal : BaseDal<Todo>
    {
        private IEnumerable<Todo> ReadManyTodo(IDataReader dataReader)
        {
            var Todos = new List<Todo>();
            while (dataReader.Read())
            {
                var Todo = new Todo();
                ReadBaseTodo(Todo, dataReader);
                Todos.Add(Todo);
            }
            return Todos;
        }

        private Todo ReadTodo(IDataReader dataReader)
        {
            var Todo = new Todo
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseTodo(Todo, dataReader);
            }
            return Todo;
        }

        private void ReadBaseTodo(Todo Todo,IDataReader dataReader)
        {
            Todo.Id = (long)dataReader["Id"];
            Todo.MemberId = (long)dataReader["MemberId"];
            Todo.Content = (string)dataReader["Content"];
            Todo.IsDone = (bool)dataReader["IsDone"];
            Todo.DoneDate = (DateTime)dataReader["DoneDate"];
            Todo.Dob = (DateTime)dataReader["Dob"];
        }

        private void CreateParameter(Todo Todo, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@MemberId", Todo.Content, DbType.Int64));
            parameters.Add(CreateParameter("@Content", 255, Todo.Content, DbType.String));
            parameters.Add(CreateParameter("@IsDone", Todo.IsDone, DbType.Boolean));
            parameters.Add(CreateParameter("@DoneDate", Todo.DoneDate, DbType.DateTime));
            parameters.Add(CreateParameter("@Dob", Todo.Dob, DbType.DateTime));
        }

        public  long Insert(Todo Todo)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(Todo, parameters);

            long lastid =  Insert("DAH_Todo_Insert", CommandType.StoredProcedure, parameters.ToArray());
            Todo.Id = lastid;
            return lastid;
        }

        public  long Update(Todo Todo)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", Todo.Id, DbType.Int64)
            };
            CreateParameter(Todo, parameters);

            return  Update("DAH_Todo_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public  long Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return  Delete("DAH_Todo_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public  Todo GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return  ReadSingleFunc("DAH_Todo_GetById", parameters, ReadTodo);
        }

        public IEnumerable<Todo> GetByIsDone(bool isDone)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@IsDone", isDone, DbType.Boolean)
            };
            return  ReadManyFunc("DAH_Todo_GetByIsDone", parameters, ReadManyTodo);
        }

        public IEnumerable<Todo> GetAll()
        {
            return  ReadManyFunc("DAH_Todo_GetAll", null, ReadManyTodo);
        }
    }
}
