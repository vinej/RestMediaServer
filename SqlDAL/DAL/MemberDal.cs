﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlDAL.Domain;
using System.Threading.Tasks;

namespace SqlDAL.DAL
{
    public class MemberDal : BaseDal<Member>
    {
        private IEnumerable<Member> ReadManyMember(IDataReader dataReader)
        {
            var members = new List<Member>();
            while (dataReader.Read())
            {
                var member = new Member();
                ReadBaseMember(member, dataReader);
                members.Add(member);
            }
            return members;
        }

        private Member ReadMember(IDataReader dataReader)
        {
            var member = new Member
            {
                Id = -1
            };
            var isData = dataReader.Read();
            if (isData)
            {
                ReadBaseMember(member, dataReader);
            }
            return member;
        }

        private void ReadBaseMember(Member member,IDataReader dataReader)
        {
            member.Id = (long)dataReader["Id"];
            member.Email = (string)dataReader["Email"];
            member.Alias = (string)dataReader["Alias"];
            member.IsActive = (bool)dataReader["IsActive"];
            member.Dob = (DateTime)dataReader["Dob"];
        }

        private void CreateParameter(Member member, List<SqlParameter> parameters)
        {
            parameters.Add(CreateParameter("@Email", 255, member.Email, DbType.String));
            parameters.Add(CreateParameter("@Alias", 255, member.Alias, DbType.String));
            parameters.Add(CreateParameter("@IsActive", member.IsActive, DbType.Boolean));
            parameters.Add(CreateParameter("@Dob", member.Dob, DbType.DateTime));
        }

        public async Task<long> Insert(Member member)
        {
            var parameters = new List<SqlParameter>();
            CreateParameter(member, parameters);

            long lastid = await InsertAsync("DAH_Member_Insert", CommandType.StoredProcedure, parameters.ToArray());
            member.Id = lastid;
            return lastid;
        }

        public async Task<long> Update(Member member)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", member.Id, DbType.Int64)
            };
            CreateParameter(member, parameters);

            return await UpdateAsync("DAH_Member_Update", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<long> Delete(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };

            return await DeleteAsync("DAH_Member_Delete", CommandType.StoredProcedure, parameters.ToArray());
        }

        public async Task<Member> GetById(long id)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Id", id, DbType.Int64)
            };
            return await ReadSingleFunc("DAH_Member_GetById", parameters, ReadMember);
        }

        public async Task<Member> GetByAlias(string alias)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@Alias", alias, DbType.String)
            };
            return await ReadSingleFunc("DAH_Member_GetByAlias", parameters, ReadMember);
        }

        public async Task<IEnumerable<Member>> GetAll()
        {
            return await ReadManyFunc("DAH_Member_GetAll", null, ReadManyMember);
        }

        public async Task<IEnumerable<Member>> GetByIsActive(bool isActive)
        {
            var parameters = new List<SqlParameter>
            {
                CreateParameter("@IsActive", isActive, DbType.Boolean)
            };
            return await ReadManyFunc("DAH_Member_GetByIsActive", parameters, ReadManyMember);
        }
    }
}