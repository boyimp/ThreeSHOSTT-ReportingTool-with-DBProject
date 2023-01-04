
using System;
using System.Data;

namespace BusinessObjects
{
    #region EnuSQL Syntax Enum
    public enum SQLSyntax
    {
        Access = 1, SQL, Oracle, Informix
    }
    #endregion
    
    #region DataAccess: MakeSQL
	public class SQLParser
	{

		#region Declaraion & Constructor
		private  SQLSyntax _sqlSyntax = SQLSyntax.SQL;
        public SQLParser(){}
		#endregion

		#region DateTime Formatting Functions
		private  string GetDateLiteral(DateTime dt) 
		{
			string s = "";
			switch (_sqlSyntax) 
			{
				case SQLSyntax.Access:
					s = "#" + dt.ToString("dd MMM yyyy") + "#";
					break;

				case SQLSyntax.Oracle:
					s = "TO_DATE('" + dt.ToString("dd MM yyyy") + "', '" + "DD MM YYYY" + "')";
					break;

				case SQLSyntax.SQL:
					s = "'" + dt.ToString("dd MMM yyyy") + "'";
					break;

                case SQLSyntax.Informix:
                    s =  string.Format("DATETIME ({0}) YEAR TO DAY", dt.ToString("yyyy-MM-dd"));
					break;
			}
			return s;
		}

		private  string GetDateTimeLiteral(DateTime dt) 
		{
			string s = "";
			switch (_sqlSyntax) 
			{
				case SQLSyntax.Access:
					s = "#" + dt.ToString("dd MMM yyyy HH:mm:ss") + "#";
					break;

				case SQLSyntax.Oracle:
					s = "TO_DATE('" + dt.ToString("dd MM yyyy HH mm ss") + "', 'DD MM YYYY HH24 MI SS')";
					break;

				case SQLSyntax.SQL:
					s = "'" + dt.ToString("dd MMM yyyy HH:mm:ss") + "'";
					break;
                
                case SQLSyntax.Informix:
                    s = string.Format("DATETIME ({0}) YEAR TO SECOND", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
            }
			return s;
		}

		private   string GetTimeLiteral(DateTime dt) 
		{
			string s = "";
			switch (_sqlSyntax) 
			{
				case SQLSyntax.Access:
					s = "#" + dt.ToString("HH:mm:ss") + "#";
					break;

				case SQLSyntax.Oracle:
					s = "TO_DATE('" + dt.ToString("HH mm ss") + "', 'HH24 MI SS')";
					break;

				case SQLSyntax.SQL:
					s = "'" + dt.ToString("HH:mm:ss") + "'";
					break;
                
                case SQLSyntax.Informix:
                    s = string.Format("DATETIME ({0}) HOUR TO SECOND", dt.ToString("HH:mm:ss"));
                    break;
            }
			return s;
		}
		#endregion

        #region Make SQL function
        /// <summary>
        /// This function returns actual T-SQL command.
        /// </summary>
        /// <param name="sqlSyntax">T-SQL command will be in Oracle, MS SQL Server or MS Access type.</param>
        /// <param name="sql">T-SQL command according to CEL style.</param>
        /// <param name="args">Comma delimited value according to CEL style.</param>
        /// <returns>Return the actual T-SQL command.</returns>
        public  string MakeSQL(SQLSyntax sqlSyntax, string sql, params object[] args)
        {
            _sqlSyntax = sqlSyntax; 
            return MakeSQL(sql, args);
        }

		/// <summary>
		/// This function returns actual T-SQL command.
		/// </summary>
		/// <param name="sql">T-SQL command according to CEL style.</param>
		/// <param name="args">Comma delimited value according to CEL style.</param>
		/// <returns>Return the actual T-SQL command.</returns>
		public  string MakeSQL(string sql, params object[] args)
		{
			string retSQL = "";
			
			if (args.Length == 0)
				return sql;

			string[] argSQL = new string[args.Length];
			int argIndex = -1;

			int i = sql.IndexOf("%");

			while (i != -1)
			{
				retSQL = retSQL + sql.Substring(0, i);
				
                if (i == sql.Length - 1)
					throw new InvalidExpressionException("Invalid Place Holder Character");

				string c = sql.Substring(i + 1, 1);
				sql = sql.Substring(i + 2);

				if (c.IndexOfAny(new char[] {'%', '{'}) != -1)
				{
					switch (c)
					{
						case "%":
							retSQL = retSQL + "%";
							break;

						case "{":
							int next = sql.IndexOf("}");
							
                            if (next < 1) 
								throw new InvalidExpressionException("Invalid Ordinal Variable");

                            int ord = Convert.ToInt32(sql.Substring(0, next));
							
                            if (ord < 0 || ord > argIndex) 
								throw new IndexOutOfRangeException("Invalid Ordinal Variable");

                            retSQL = retSQL + argSQL[ord];
							sql = sql.Substring(next + 1);
							break;
					}
				}
				else if (c.IndexOfAny(new char[] {'s', 'n', 'd', 't', 'D', 'b', 'q'}) != -1)
				{
					if (++argIndex > args.Length - 1)
						throw new ArgumentException("Too few arguments passed");


					if (args[argIndex] == null)
						argSQL[argIndex] = "NULL";
					else
					{
						try
						{
							switch (c)
							{
								case "s":
									string s = Convert.ToString(args[argIndex]);
									argSQL[argIndex] = "'" + s.Replace("'", "''") + "'";
									break;

								case "n":
									decimal n = Convert.ToDecimal(args[argIndex]);
									argSQL[argIndex] = n.ToString();
									break;

								case "d":
									DateTime d = Convert.ToDateTime(args[argIndex]);
									argSQL[argIndex] = GetDateLiteral(d);
									break;

								case "t":
									DateTime t = Convert.ToDateTime(args[argIndex]);
									argSQL[argIndex] = GetTimeLiteral(t);
									break;

								case "D":
									DateTime D = Convert.ToDateTime(args[argIndex]);
									argSQL[argIndex] = GetDateTimeLiteral(D);
									break;

								case "b":
									bool b = Convert.ToBoolean(args[argIndex]);
									if (_sqlSyntax == SQLSyntax.Access)
										argSQL[argIndex] = b.ToString();
									else
										argSQL[argIndex] = (b ? "1" : "0");
									break;

								case "q":
									string q = Convert.ToString(args[argIndex]);
									argSQL[argIndex] = q;
									break;
							}
						}
						catch 
						{
							throw new ArgumentException("Invalid argument #" + argIndex);
						}
					}
					retSQL = retSQL + argSQL[argIndex];
				}
				else
					throw new InvalidExpressionException("Invalid Place Holder Character");

                i = sql.IndexOf("%");
			}
			retSQL = retSQL + sql;

			// Handle the (==)
			i = retSQL.IndexOf("==");
			while (i != -1)
			{
				string rVal = retSQL.Substring(i + 2).Trim().Substring(0);
				if (rVal.ToUpper().StartsWith("NULL"))
					retSQL = retSQL.Substring(0, i) + " IS " + retSQL.Substring(i + 2);
				else
					retSQL = retSQL.Substring(0, i) + "=" + retSQL.Substring(i + 2);

				i = retSQL.IndexOf("==", i + 2);
			}

			// Handle the (<>)
			i = retSQL.IndexOf("<>");
			while (i != -1)
			{
				string rVal = retSQL.Substring(i + 2).Trim().Substring(1);
				if (rVal.ToUpper().StartsWith("NULL"))
					retSQL = retSQL.Substring(0, i) + " IS NOT " + retSQL.Substring(i + 2);

                i = retSQL.IndexOf("<>", i + 2);
			}

			return retSQL;
		}
		#endregion

		#region TagSQL Function
        /// <summary>
        /// This function return T-SQL command by adding WHERE/AND depending on commandText.
		/// </summary>
		/// <param name="commandText">Previous T-SQL command.</param>
		/// <returns>Return T-SQL command after addition of WHERE/AND maintaining the space.</returns>
		public  string TagSQL(string commandText)
		{
			return string.Format("{0} {1} ", commandText.Trim(), (commandText.Trim().Length <= 0 ? "WHERE" : "AND")); 
		}
		#endregion
	}
	#endregion
}