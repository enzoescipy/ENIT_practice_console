using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{


    class Class1
    {
        static void Main(string[] args)     //메뉴에서 비밀번호 byuk 으로 관리자메뉴 들어가기.
        {
            //읽기 
            SettingData settingData = new SettingData();
            settingData.readUser();


            /*
             쓰기 
            List<UserVO> userList = new List<UserVO>();
            UserVO user = new UserVO("id", "1234", "snvf", "22", "010", "jeju", 0);
            userList.Add(user);
            settingData.UpdataUserData(userList);
            */
        }
    }


    class SettingData
    {

        public SettingData() { }


        public void UpdataUserData(List<UserVO> userList)
        {
            Stream ws = new FileStream("userInfomation.dat", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(ws, userList);     //직렬화(저장)
            ws.Close();
        }


        public void readUser()
        {

            List<UserVO> userList = new List<UserVO>(); // UserVO 리스트

            Stream ws;
            FileInfo fileUserInfo = new FileInfo("userInfomation.dat");
            if (!fileUserInfo.Exists)       //파일이 없을경우
            {
                ws = new FileStream("userInfomation.dat", FileMode.Create);
                ws.Close();
            }
            else
            {
                if (fileUserInfo.Length != 0)   //기존의 데이타를 가지고 있다면.
                {
                    Stream rs = new FileStream("userInfomation.dat", FileMode.Open); //일단 불러온다.
                    BinaryFormatter deserializer = new BinaryFormatter();
                    userList = (List<UserVO>)deserializer.Deserialize(rs);       //역직렬화,리스트에 저장함.

                    Console.Write(userList[0].UserName);
                    rs.Close();
                }
            }


        }
        }
}
