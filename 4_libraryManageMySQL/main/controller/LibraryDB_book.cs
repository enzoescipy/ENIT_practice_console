namespace MainProject
{
    public class LibraryDB_Book
    {
        public LibraryDB_Book(LocalDBmodel<BookVO> localDBmodel) 
        {
            this.localDBmodel = localDBmodel;
        }
        private LocalDBmodel<BookVO> localDBmodel;


        /// <summary>
        /// validate if bookVO can be added to the book DB.
        /// </summary>
        /// <param name="bookVO"></param>
        /// <returns>
        /// return 0 : success code (can be added to DB)
        /// return -1 : nullable VO.
        /// return 1 : name is already in the other VO inside of db.
        /// return 2 : some field is the blank value.
        /// </returns>
        public int BookValidate(BookVO bookVO)
        {
            // validate if name is new name
            var query = localDBmodel.Find("name", bookVO.name, 0);
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
        /// return 1 : name is are already in the other VO inside of db.
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
            var query = localDBmodel.Find("name", "description", keyword);
            return query;
        }

        public BookVO? BookSearchExact(string name)
        {
            // find the keyword in the DB
            var nameQuery = localDBmodel.Find("name", name, 0);
            if (nameQuery.Count == 0) { return null; }
            return nameQuery[0];
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
        /// return 1 : stock change failed
        /// </returns>
        /// <exception cref="InvalidDataException"></exception>
        public int BookInfoChange(string targetName, string? name, string? description, int? initStock)
        {
            // find pKey book
            var query = localDBmodel.Find("name", targetName, 0);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("primaryKey-matching VO must be unique."); }

            
            // if param is not null, then change the field of VO.
            if (name != null) {query[0].name = name;}
            if (description != null) {query[0].description = description;}

            // stock logic verify
            if (initStock != null) 
            {
                // check for book's currentStock
                int currentStockBefore = query[0].currentStock;
                int initStockBefore = query[0].initStock;
                int initStockDiff = (int) initStock - initStockBefore;

                // initStock can't be decrease over than currentStock.
                if (initStockDiff < - currentStockBefore) { return 1; }

                // change currentStock and initStock simultaneously
                query[0].currentStock = (int) currentStockBefore + initStockDiff;
                query[0].initStock = (int) initStock;
            }

            // overrides the VO
            localDBmodel.Override(query);
            return 0;
        }

        // /// <summary>
        // /// borrow targetNamed book by amount
        // /// if amount over currentstock of book, reject.
        // /// </summary>
        // /// <param name="targetName"></param>
        // /// <param name="amount"></param>
        // /// <returns>
        // /// return 0 : succeed
        // /// return 1 : invalid amount
        // /// return -1 : no book found
        // /// </returns>
        // /// <exception cref="InvalidDataException"></exception>
        // public int BookBorrow(string booktName, int amount)
        // {
        //     // find pKey book
        //     var query = localDBmodel.Find("name", booktName, 0);
        //     if (query.Count == 0) { return -1; }
        //     else if (query.Count > 1) { throw new InvalidDataException("primaryKey-matching VO must be unique."); }

        //     // change currentStock
        //     if (amount > query[0].currentStock) {return 1;}
        //     query[0].currentStock = query[0].currentStock - amount;

        //     // overrides the VO
        //     localDBmodel.Override(query);
        //     return 0;
        // }

        // public int BookReturn(string booktName, int amount)
        // {
        //     // find pKey book
        //     var query = localDBmodel.Find("name", booktName, 0);
        //     if (query.Count == 0) { return -1; }
        //     else if (query.Count > 1) { throw new InvalidDataException("primaryKey-matching VO must be unique."); }

        //     // change currentStock
        //     if (amount > query[0].initStock - query[0].currentStock) {return 1;}
        //     query[0].currentStock = query[0].currentStock + amount;
            
        //     // overrides the VO
        //     localDBmodel.Override(query);
        //     return 0;
        // }

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
            List<BookVO> query = localDBmodel.Find("name", name, 0);
            if (query.Count == 0) { return -1; }
            else if (query.Count > 1) { throw new InvalidDataException("name-matching VO must be unique."); }
            
            // get the book's pKey.
            List<int> pKeyDelete = new List<int>() {query[0].primaryKey};

            //delete all log related to this book
            LibraryDB.libraryDB.log.DeleteLogForBook(name);

            // do the delete 
            localDBmodel.Delete(pKeyDelete);
            return 0;
        }
    }
}