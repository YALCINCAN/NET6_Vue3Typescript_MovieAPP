import { ref, watch } from 'vue';

export function useImageUpload() {
  const imagefile = ref(null);
  const previewimageurl = ref<string>('');

  watch(
    () => imagefile.value,
    () => {
      if (imagefile.value != null) {
        const fileReader = new FileReader();
        fileReader.readAsDataURL(imagefile.value);
        fileReader.addEventListener('load', () => {
          previewimageurl.value = fileReader.result as string
        });
      }
      previewimageurl.value = '';
    }
  );

  return {
    imagefile,
    previewimageurl,
  };
}
