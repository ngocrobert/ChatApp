using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        // items-dsach ptu cua trang hien tai; count-tong so ptu; pageNumber-so cua trang htai; pageSize-so luong ptu moi trang
        public PagedList(IEnumerable<T> items ,int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize); //tinh tong so trang
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        /// <summary>
        /// thực hiện phân trang từ một nguồn dữ liệu (IQueryable<T>) theo cách lấy dữ liệu từng phần từ database, tránh lấy tất cả dữ liệu cùng một lúc
        /// </summary>
        /// <param name="source">Một IQueryable<T> chứa toàn bộ dữ liệu ban đầu</param>
        /// <param name="pageNumber">Số trang hiện tại cần lấy</param>
        /// <param name="pageSize">Số lượng phần tử mỗi trang</param>
        /// <returns>trả về một đối tượng PagedList<T></returns>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync(); // Đếm tổng số phần tử trong source
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
