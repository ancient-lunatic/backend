using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using eleven.Models;

using MySql.Data.MySqlClient;


namespace eleven
{
    public class employee
    {
        
        public appDb Db { get; }
       
        public employee(appDb db)
        {
            Db = db;
        }



        // querry execute
        public async Task<List<employees>> getUserInformation()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select employee.employeeId , firstName  , middleName , lastname ,countryName as nationality,
CONCAT(addressLine1 , ',' , addressLine2 , ' ,' , cityName,',',stateName,',',pincode)as address,GROUP_CONCAT(contact_information.contactNumber) as `numbers`,
TIMESTAMPDIFF(YEAR,dateOfJoing,CURDATE()) as experienceyaer, TIMESTAMPDIFF( MONTH, dateOfJoing, now() ) % 12 as currentExperiencemonth,
    TIMESTAMPDIFF ( YEAR, birthDate, now() ) as year
    , TIMESTAMPDIFF( MONTH, birthDate, now() ) % 12 as month
    , ( TIMESTAMPDIFF( DAY, birthDate, now() ) % 30 ) as day
FROM employee,address,join_date,city,country,contact_information,state where employee.employeeId=address.addressId and
address.cityId=city.cityId and employee.employeeId=join_date.employeeId and employee.nationality=country.countryId and state.stateId=city.stateId
 and employee.employeeId=contact_information.employeeId group by
  employee.employeeId;";

            return await ReadAllEmployee(await cmd.ExecuteReaderAsync());
        }



        public async Task<List<employees>> FindOneAsync(string id)

        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from (select employee.employeeId ,  firstName  , middleName , lastname ,countryName as nationality,
CONCAT(addressLine1 , ', ' , addressLine2 , ' , ' , ' ' , cityName,', ' , ' ',stateName,', ' , ' ',pincode)as address,GROUP_CONCAT(contact_information.contactNumber) as `numbers`,
TIMESTAMPDIFF(YEAR,dateOfJoing,CURDATE()) as experienceyaer, TIMESTAMPDIFF( MONTH, dateOfJoing, now() ) % 12 as currentExperiencemonth,
    TIMESTAMPDIFF ( YEAR, birthDate, now() ) as year
    , TIMESTAMPDIFF( MONTH, birthDate, now() ) % 12 as month
    , ( TIMESTAMPDIFF( DAY, birthDate, now() ) % 30 ) as day
FROM employee,address,join_date,city,country,contact_information,state where employee.employeeId=address.addressId and
address.cityId=city.cityId and employee.employeeId=join_date.employeeId and employee.nationality=country.countryId and state.stateId=city.stateId
 and employee.employeeId=contact_information.employeeId group by
  employee.employeeId)as sun WHERE firstName LIKE '" + @id + "%'";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = id,
            });
            return await ReadAllEmployee(await cmd.ExecuteReaderAsync());
            //return result.Count > 0 ? result[0] : null;

            
        }


        public async Task insertAsync(insertData result)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"insert into employee(salutationId,firstName,middleName,lastName,employeeGenderId,birthDate)values
(32,@salutation,@firstName,@middleName,@lastName,@gender,@date);
set @last_id=LAST_INSERT_ID();
insert into address(employeeId,addressLine1,
addressLine2,
landmark,cityId)
 values(@last_id,@addressLine1,@addressLine2,@Locality,@city);
 insert into contact_information(employeeId,contactNumber)
 values(@last_id,@contact);";
            BindParamsDATA(cmd, result.salutation, result.firstName, result.MiddleName, result.lastName, result.gender , result.date , result.contact , result.city);
            BindId(cmd, result.addressLine1, result.addressLine2, result.Locality);
            //bindParams

            cmd.CommandText = cmd.CommandText;

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<employees>> delete(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"delete from employee where employee.employeeId = @id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            return await ReadAllEmployee(await cmd.ExecuteReaderAsync());
        }



        public async Task UpdateAsync(editDetails result)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE employee
SET
employee.salutationId=@salutation,
employee.firstname = @firstName,
employee.middlename=@middleName,
employee.lastname=@lastName,
employee.employeeGenderId=@gender
where employee.employeeId=@id ;
                            UPDATE address					
                            SET
                            address.addressLine1= @addressLine1,
                            address.addressLine2=@addressLine2,
                            landmark=@locality
                            WHERE address.employeeId=@id;";
            BindParams(cmd, result.Id , result.salutation ,  result.firstName, result.MiddleName , result.lastName,  result.gender );
            BindId(cmd, result.addressLine1 , result.addressLine2 , result.Locality );
            //bindParams

            cmd.CommandText = cmd.CommandText;

            await cmd.ExecuteNonQueryAsync();
        }


        private void BindId(MySqlCommand cmd, string addressLine1 , string addressLine2 , string locality)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@addressLine1",
                DbType = DbType.String,
                Value = addressLine1,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@addressLine2",
                DbType = DbType.String,
                Value = addressLine2,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@locality",
                DbType = DbType.String,
                Value = locality,
            });
            
        }


        
        private void BindParams(MySqlCommand cmd, int Id , int salutation , string firstName, string middleName , string lastName ,  int gender)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.Int32,
                Value = Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@salutation",
                DbType = DbType.Int32,
                Value = salutation,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = firstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@middleName",
                DbType = DbType.String,
                Value = middleName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastName",
                DbType = DbType.String,
                Value = lastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@gender",
                DbType = DbType.Int32,
                Value = gender,
            });
        }
        private void BindParamsDATA(MySqlCommand cmd, int salutation, string firstName, string middleName, string lastName, int gender , DateTime date ,  string contact , int city)
        {
           
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@salutation",
                DbType = DbType.Int32,
                Value = salutation,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = firstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@middleName",
                DbType = DbType.String,
                Value = middleName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastName",
                DbType = DbType.String,
                Value = lastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@gender",
                DbType = DbType.Int32,
                Value = gender,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@city",
                DbType = DbType.Int32,
                Value = city,
            });
          /*  cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nationlaity",
                DbType = DbType.Int32,
                Value = nationlaity,
            });*/
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@date",
                DbType = DbType.Date,
                Value = date,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@contact",
                DbType = DbType.String,
                Value = contact,
            });
        }


        public async Task<editDetails> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select e.employeeId , e.salutationId , e.firstName , e.middleName ,e.LastName ,ad.addressLine1 , ad.addressLine2 , ad.landmark , ad.cityId , ci.stateId , s.countryId 
from employee as e 
inner join salutation as sl 
inner join address as ad 
inner join city as ci 
inner join state as s 
inner join country as c  on sl.salutationId = e.salutationId  and e.employeeId = ad.employeeId and ci.cityId = ad.cityId and ci.stateId = s.stateId and s.countryId = c.countryId where e.employeeId = @id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await readOneAsync(await cmd.ExecuteReaderAsync());
            return result!=null? result : null;

        }




        private async Task<editDetails> readOneAsync(DbDataReader reader)
        {

            editDetails post;
            using (reader)
            {
                if (await reader.ReadAsync())
                {
                    post = new editDetails()
                    {
                        Id = reader.GetInt32(0),
                        salutation = reader.GetInt32(1),
                        firstName = reader.GetString(2),
                        MiddleName = reader.GetString(3),
                        lastName = reader.GetString(4),
                        addressLine1 = reader.GetString(5),
                        addressLine2 = reader.GetString(6),
                        Locality = reader.GetString(7),
                        
                    };
                    return post;
                }
            }
            return null;
        }


        private async Task<List<editDetails>> readtwoAsync(DbDataReader reader)
        {

            var posts = new List<editDetails>();   // create an array of blogpost
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new editDetails()
                    {
                        Id = reader.GetInt32(0),
                        salutation = reader.GetInt32(1),
                        firstName = reader.GetString(2),
                        MiddleName = reader.GetString(3),
                        lastName = reader.GetString(4),
                        addressLine1 = reader.GetString(5),
                        addressLine2 = reader.GetString(6),
                        Locality = reader.GetString(7)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }




        //mapping   





        public async Task<List<employees>> ReadAllEmployee(DbDataReader reader)
        {
            var posts = new List<employees>();   // create an array of blogpost
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new employees()
                    {   
                        userId = reader.GetInt16(0),
                        Name = reader.GetString(1) + " "+((reader.GetString(2)=="" || reader.GetString(2) == null) ? "" : reader.GetString(2)+ " " )+"" + reader.GetString(3),
                        //reader.GetString(0)
                        nationality = reader.GetString(4),
                        Address = reader.GetString(5),
                        contactDetail = new contactInformation(reader.GetString(6)),
                        currentCompanyExp = reader.GetString(7)+ " years " + reader.GetString(8) + " months",
                        Age = new Models.dateOfBirth(reader.GetString(9) , reader.GetString(10) , reader.GetString(11)),
                       

                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

      
    }
}
