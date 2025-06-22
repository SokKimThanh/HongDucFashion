using HongDucFashion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HongDucFashion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly HongDucFashionV1Context _db;

        public ProductsController(HongDucFashionV1Context db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _db.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try
            {
                _db.Products.Add(product);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }
            catch (DbUpdateException ex) when (IsDuplicateKeyException(ex))
            {
                return Conflict(new { Message = "A product with the same key already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            // Kiểm tra ID trên URL và ID trong body có khớp không
            if (id != product.ProductId)
            {
                return BadRequest(new { Message = "Product ID mismatch." });
            }

            // Đánh dấu entity là đã bị chỉnh sửa
            _db.Entry(product).State = EntityState.Modified;
            try
            {
                // Lưu thay đổi vào database
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Nếu không tìm thấy sản phẩm để cập nhật
                if (!ProductExists(id))
                {
                    return NotFound(new { Message = "Product not found." });
                }
                else
                {
                    throw; // Lỗi khác, ném lại exception
                }
            }
            catch (DbUpdateException ex) when (IsDuplicateKeyException(ex))
            {
                // Nếu bị trùng khóa chính/duy nhất
                return Conflict(new { Message = "A product with the same key already exists." });
            }
            return NoContent(); // Thành công, không trả về dữ liệu
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products
                .Include(p => p.OrderDetails)
                .Include(p => p.InventoryTransactions)
                .Include(p => p.Promotions)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }

            // Kiểm tra quan hệ với OrderDetail
            if (product.OrderDetails != null && product.OrderDetails.Any())
            {
                return BadRequest(new { Message = "Cannot delete product because it is referenced in order details." });
            }

            // Kiểm tra quan hệ với InventoryTransaction
            if (product.InventoryTransactions != null && product.InventoryTransactions.Any())
            {
                return BadRequest(new { Message = "Cannot delete product because it is referenced in inventory transactions." });
            }

            // Kiểm tra quan hệ với Promotion (nếu cần)
            if (product.Promotions != null && product.Promotions.Any())
            {
                return BadRequest(new { Message = "Cannot delete product because it is referenced in promotions." });
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok(new { Message = "Product deleted successfully." });
        }

        private bool ProductExists(int id)
        {
            return _db.Products.Any(e => e.ProductId == id);
        }

        private static bool IsDuplicateKeyException(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                return true;
            }
            return false;
        }
    }
}
