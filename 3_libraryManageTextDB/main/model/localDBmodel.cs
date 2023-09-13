using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace MainProject
{
    public class LocalDBmodel<VO>
        where VO : BasicVO, new()
    {
        public string dirDB; 
        private int PkeyCounter;
        public LocalDBmodel(string dirDB)
        {
            this.dirDB = dirDB;

            // set PkeyCounter
            var currentDB = Get();
            if (currentDB == null)
            {
                this.PkeyCounter = 0;
            }
            else
            {
                int maxKey = 0;
                foreach (var row in currentDB)
                {
                    var pkey = row.primaryKey;
                    if (maxKey < pkey)
                    {
                        maxKey = pkey;
                    }
                }
                this.PkeyCounter = maxKey + 1;
            }
        }

        private void debug(List<VO> v)
        {

        }

        /// <summary>
        /// inspect if current dirDB directory is correct and DB is not empty.
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            FileInfo fileInfo = new FileInfo(dirDB);
            if (fileInfo.Exists && fileInfo.Length != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// create new db from voInstanceList
        /// if db already exists, override it.
        /// </summary>
        /// <param name="voInstanceList"></param>
        private void Create(List<VO> voInstanceList)
        {
            // HasNull inspectation
            foreach (VO vo in voInstanceList)
            {
                if (vo.HasNull()) {throw new NullReferenceException("VO instances must be Not-Has null state. are you missing to put value in some VOs?");}
            }
            Stream s = new FileStream(dirDB, FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(s, voInstanceList);
            s.Close();
        }

        /// <summary>
        /// get the all of VO in db.
        /// </summary>
        /// <returns>
        /// return List<VO> : success
        /// return null : there's no db
        /// </returns>
        private List<VO>? Get()
        {
            // existance check
            if (!IsValid()) {return null;}

            // get List<vo>
            Stream s = new FileStream(dirDB, FileMode.Open);
            BinaryFormatter serializer = new BinaryFormatter();
            var rid = (List<VO>) serializer.Deserialize(s);
            // //debug
            // DebugConsole.D1List(rid);
            // //debug
            s.Close();
            return rid;
        }

        private int PkeyCounterGet()
        {
            int currentCounter = PkeyCounter;
            PkeyCounter++;
            return currentCounter;
        }

        /// <summary>
        /// get the all of VO in db then return.
        /// </summary>
        /// <returns></returns>
        public List<VO> GetAll()
        {
            var rid = Get();
            if (rid == null)
            {
                return new List<VO>();
            }
            else
            {
                return rid;
            }
        }

        /// <summary>
        /// append newVOList to the current DB row.
        /// can create new DB. can be called if current DB file not exists or empty.
        /// </summary>
        /// <param name="newVOList"></param>
        public void Append(List<VO> newVOList)
        {
            // adjust primary key
            for (int i = 0; i < newVOList.Count; i++)
            {
                newVOList[i].primaryKey = PkeyCounterGet();
            }

            // save DB
            var dbVOList = Get();
            if (dbVOList == null)
            {
                Create(newVOList);
            }
            else
            {
                Create(dbVOList.Concat(newVOList).ToList());
            }
        }

        /// <summary>
        /// find the matching property of the targetVO which is not null, in the DB then return the VO List.
        /// </summary>
        /// <param name="targetVO"></param>
        /// <returns></returns>
        public List<VO> Find(string fieldName, dynamic fieldValue, Func<dynamic, dynamic, bool> compareFunc)
        {
            // get fieldInfo of fieldname. if no field that name, return empty list.
            var fieldInfo = typeof(VO).GetField(fieldName);
            if (fieldInfo == null) { return new List<VO>(); }

            // get current VO db.
            var dbVOList = Get();

            if (dbVOList == null) { return new List<VO>(); }

            // search for targeted field.
            var newVOList = new List<VO>();

            foreach (VO vo in dbVOList)
            {
                Console.WriteLine(fieldInfo.GetValue(vo));
                if (compareFunc(fieldInfo.GetValue(vo), fieldValue))
                {
                    newVOList.Add(vo);
                }
            }

            return newVOList;


        }


        /// <summary>
        /// override the DB by the VO in the newVOList, which has same Pkey.
        /// if Pkey not matched, just skip.
        /// </summary>
        /// <param name="newVOList"></param>
        public void Override(List<VO> newVOList)
        {
            var dbVOList = Get();
            var applyVOList = new List<VO>();
            if (dbVOList == null)
            {
                return;
            }
            else
            {
                foreach (VO newVO in newVOList)
                {
                    // check if newVO 's primary key is in the DB 
                    int selectedIndex = -1;
                    for (int i = 0; i < dbVOList.Count; i++ )
                    {
                        var selectedVO = dbVOList[i];
                        if (selectedVO.primaryKey == newVO.primaryKey)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }

                    // if primaryKey in DB then delete it from db then store it both temporary
                    if (selectedIndex != -1)
                    {
                        dbVOList.RemoveAt(selectedIndex);
                        applyVOList.Add(newVO);
                    }
                }

                dbVOList = dbVOList.Concat(applyVOList).ToList();
                Create(dbVOList);
            }
        }

        public void Delete(List<int> deletePkeyList)
        {
            var dbVOList = Get();
            if (dbVOList == null)
            {
                return;
            }

            for (int i=0; i < dbVOList.Count; i++)
            {
                var vo = dbVOList[i];
                if (deletePkeyList.Contains(vo.primaryKey))
                {
                    dbVOList.RemoveAt(i);
                    i--;
                }
            }
            Create(dbVOList);
        }


    }
}