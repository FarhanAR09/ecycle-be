﻿using ecycle_be.Models;
using ecycle_be.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static ecycle_be.Services.ProductService;

namespace ecycle_be.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController(ProductService productService) : ControllerBase
    {
        private readonly ProductService _productService = productService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Produk> products = await _productService.GetProductsThumbnails();
                var data = products.Select(p => new
                {
                    p.ProdukID,
                    p.Nama,
                    p.Harga
                });
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                DetailedUIProduk produk = await _productService.GetById(id);
                return Ok(produk);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUser(int id)
        {
            try
            {
                List<Produk> produk = await _productService.GetByUser(id);
                return Ok(produk);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostProduct([FromBody] Produk produk)
        {
            try
            {
                Produk postedProduct = await _productService.PostProduk(produk);
                return Ok(postedProduct);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateProduct(Produk produk)
        {
            try
            {
                await _productService.PatchProduk(produk);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPatch("beli")]
        public async Task<IActionResult> Beli(PembelianProduk beli)
        {
            try
            {
                await _productService.Beli(beli);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(PembelianProduk produk)
        {
            try
            {
                await _productService.Delete(produk);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
