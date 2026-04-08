using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace QA_TMDT.Helper
{
    public class JsonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Lấy tất cả các giá trị cho key này (ví dụ: DsBienThe)
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            // Lấy danh sách các string gửi lên
            var values = valueProviderResult.Values;

            if (values.Count == 0)
            {
                return Task.CompletedTask;
            }

            try
            {
                // TRƯỜNG HỢP 1: Gửi lên 1 chuỗi duy nhất
                if (values.Count == 1)
                {
                    var firstValue = values.First();
                    // Nếu là chuỗi rỗng thì bỏ qua
                    if (string.IsNullOrWhiteSpace(firstValue)) return Task.CompletedTask;

                    // Thử deserialize trực tiếp (trường hợp gửi mảng JSON chuẩn: "[{...}, {...}]")
                    try
                    {
                        var result = JsonConvert.DeserializeObject(firstValue, bindingContext.ModelType);
                        bindingContext.Result = ModelBindingResult.Success(result);
                        return Task.CompletedTask;
                    }
                    catch (JsonSerializationException)
                    {
                        // Nếu thất bại (do gửi object lẻ "{...}" thay vì array), ta sẽ xử lý ở dưới
                    }
                }

                // TRƯỜNG HỢP 2: Gửi nhiều chuỗi (Swagger Add String Item) HOẶC 1 object lẻ
                // Ta sẽ gom tất cả thành một chuỗi mảng JSON hợp lệ: "[{...}, {...}]"

                // Gom các chuỗi lại, phân cách bằng dấu phẩy
                var mergedJson = "[" + string.Join(",", values) + "]";

                var listResult = JsonConvert.DeserializeObject(mergedJson, bindingContext.ModelType);
                bindingContext.Result = ModelBindingResult.Success(listResult);
            }
            catch (Exception ex)
            {
                // Thêm lỗi vào ModelState để debug dễ hơn
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"Lỗi parse JSON: {ex.Message}");
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
