namespace MainProject
{
    public class LibraryDB_Book
    {
        public LibraryDB_Book(LocalDBmodel<BookVO> localDBmodel) 
        {
            this.localDBmodel = localDBmodel;
        }
        private LocalDBmodel<BookVO> localDBmodel;
        private Func<dynamic, dynamic, bool> stringEquals = (d, k) => String.Equals(d, k);
        private Func<dynamic, dynamic, bool> simpleEquals = (d, k) => d == k;
        private bool KeywordSearch(dynamic targetString, dynamic keyword)
        {
            string sTargetString = targetString.ToString();
            string sKeyword = keyword.ToString();

            return sTargetString.Contains(sKeyword);
        }

        /// <summary>
        /// validate if bookVO can be added to the book DB.
        /// </summary>
        /// <param name="bookVO"></param>
        /// <returns>
        /// return 0 : success code (can be added to DB)
        /// return -1 : nullable VO.
        /// return 1 : some field are already in the other VO inside of db.
        /// return 2 : some field is the blank value.
        /// </returns>
        public int BookValidate(BookVO bookVO)
        {
            // validate if hasNull.
            if (bookVO.HasNull()) { return -1; }

            // validate if name is new name
            var query = localDBmodel.Find("name", bookVO.name, stringEquals);
            if (query.Count != 0) { return 1; }

            // validate if name is not empty
            if (bookVO.name.Length == 0) { return 2; }

            // validate if description is not empty
            if (bookVO.description.Length == 0) { return 2; }

            // validate if initStock is over 0
            if (bookVO.initStock <= 0) { return 2; }

            return 0;

        }

        /// <summary>
        /// Add the new book to DB.
        /// </summary>
        /// <param name="bookVO"></param>
        /// <returns>
        /// return 0 : success code
        /// return -1 : nullable VO.
        /// return 1 : some field are already in the other VO inside of db.
        /// return 2 : some field is the blank value.
        /// </returns>
        public int BookAdd(BookVO bookVO)
        {
            int validate = BookValidate(bookVO);
            if (validate != 0) {return validate; };
            // override currentStock to initStock
            bookVO.currentStock = bookVO.initStock;

            // append DB.
            localDBmodel.Append(new List<BookVO>() {bookVO});
            return 0;
        }

        /// <summary>
        /// stringify the BookVO List to 2D list of string.
        /// </summary>
        /// <param name="voTable"></param>
        /// <returns></returns>
        public List<List<string>> ListVOStringify(List<BookVO> voTable)
        {
            List<string> tableColumn = new List<string>() {"NAME", "STOCK","AVAILABLE", "DESCRIPTION"};
            List<List<string>> showableTable = new List<List<string>>() {tableColumn};

            foreach(BookVO book in voTable)
            {
                List<string> line = new List<string>();
                line.Add(book.name);
                line.Add(book.initStock.ToString());
                line.Add(book.currentStock.ToString());
                line.Add(book.description);

                showableTable.Add(line);
            }

            return showableTable;
        }

        public List<BookVO> BookAll()
        {
            return localDBmodel.GetAll();
        }

        public List<List<string>> BookAllString()
        {
            return ListVOStringify(BookAll());
        }

        public List<BookVO> BookSearch(string keyword)
        {
            // find the keyword in the DB
            var nameQuery = localDBmodel.Find("name", keyword, KeywordSearch);
            var descriptionQuery = localDBmodel.Find("description", keyword, KeywordSearch);
            
            // remove the same VO from descriptionQuery
            foreach (BookVO nameQ in nameQuery)
            {
                for (int i = 0; i < descriptionQuery.Count; i++)
                {
                    BookVO descQ = descriptionQuery[i];
                    if (descQ.primaryKey == nameQ.primaryKey)
                    {
                        descriptionQuery.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            // return the result
            return nameQuery.Concat(descriptionQuery).ToList();
        }
        
        /// <summary>
        /// find VO by pKey then change info from the param.
        /// if param is null, will keep the field value.
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="initStock"></param>
        /// <param name="currentStock"></param>
        /// <returns>
        /// return 0 : success
        /// return -1 : pKey not found
        /// </returns>
        /// <exception cref="InvalidDataException"></exception>
        public int BookInfoChange(int pKey, string? name, string? description, int? initStock, int? currentStock)
        {
            // find pKey book
            var query = localDBmodel.Find("primaryKey", pKey, simpleEquals);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("primaryKey-matching VO must be unique."); }
            
            // if param is not null, then change the field of VO.
            if (name != null) {query[0].name = name;}
            if (description != null) {query[0].description = description;}
            if (initStock != null) {query[0].initStock = initStock;}
            if (currentStock != null) {query[0].currentStock = currentStock;}

            // overrides the VO
            localDBmodel.Override(query);
            return 0;
        }

        /// <summary>
        /// delete book with matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// return 0 : success
        /// return -1 : no book
        /// </returns>
        /// <exception cref="InvalidDataException"></exception>
        public int bookDelete(string name)
        {
            // find book.
            List<BookVO> query = localDBmodel.Find("name", name, simpleEquals);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("name-matching VO must be unique."); }
            
            // get the book's pKey.
            List<int> pKeyDelete = new List<int>() {query[0].primaryKey};

            // do the delete 
            localDBmodel.Delete(pKeyDelete);
            return 0;
        }
    }
}