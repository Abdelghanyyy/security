from stegano import lsb
from PIL import Image
import os
import numpy as np

def encode_message(image_path, message, output_path):
    """Encode a message into an image using stegano."""
    try:
        # Verify input image exists
        if not os.path.exists(image_path):
            raise FileNotFoundError("Input image not found")
        
        # Ensure output is PNG
        if not output_path.lower().endswith('.png'):
            raise ValueError("Output image must be in PNG format")
        
        # Encode message using LSB
        secret = lsb.hide(image_path, message)
        
        # Save the output image
        secret.save(output_path, format='PNG')
        print(f"Message encoded successfully! Saved to {output_path}")
        return True
    except Exception as e:
        print(f"Error encoding message: {e}")
        return False

def decode_message(image_path):
    """Decode a message from an image using stegano."""
    try:
        # Verify input image exists
        if not os.path.exists(image_path):
            raise FileNotFoundError("Input image not found")
        
        # Try decoding with stegano
        message = lsb.reveal(image_path)
        if message:
            print("Successfully decoded using stegano.lsb.reveal")
            return message
        else:
            print("No message found using stegano.lsb.reveal, attempting manual decode")
            return manual_decode_message(image_path)
    except Exception as e:
        print(f"Error decoding with stegano.lsb.reveal: {e}")
        print("Attempting manual decode")
        return manual_decode_message(image_path)

def manual_decode_message(image_path):
    """Manually decode a message from an image by extracting LSBs."""
    try:
        # Open image and convert to RGB
        img = Image.open(image_path).convert('RGB')
        img_array = np.array(img)
        
        # Extract LSBs from the first channel (R) of each pixel
        bits = []
        for i in range(img_array.shape[0]):
            for j in range(img_array.shape[1]):
                bits.append(img_array[i, j, 0] & 1)  # Extract LSB from R channel
                
                # Stop after a reasonable number of bits to avoid infinite loops
                if len(bits) >= 10000:  # Arbitrary limit, adjust as needed
                    break
            if len(bits) >= 10000:
                break
        
        # Convert bits to text (assuming 8-bit ASCII with null terminator)
        message = ""
        for i in range(0, len(bits), 8):
            byte = bits[i:i+8]
            if len(byte) < 8:
                break
            char_code = int(''.join(map(str, byte)), 2)
            if char_code == 0:  # Null terminator
                break
            if 32 <= char_code <= 126:  # Printable ASCII characters
                message += chr(char_code)
            else:
                break  # Stop if non-printable character is found
        return message if message else "No valid message found in manual decode"
    except Exception as e:
        print(f"Error in manual decode: {e}")
        return ""

def main():
    print("Steganography Program using Stegano")
    print("Required libraries: stegano, Pillow, numpy")
    print("Install using: pip install stegano Pillow numpy")
    print("Download links:")
    print("- Stegano: https://pypi.org/project/stegano/")
    print("- Pillow: https://pypi.org/project/Pillow/")
    print("- NumPy: https://pypi.org/project/numpy/")
    print("Note: Use PNG images for best results.")
    
    while True:
        print("\nMenu:")
        print("1. Encode message into image")
        print("2. Decode message from image")
        print("3. Exit")
        
        choice = input("Enter your choice (1-3): ")
        
        if choice == '1':
            image_path = input("Enter input image path (PNG format recommended): ")
            message = input("Enter message to hide: ")
            output_path = input("Enter output image path (must be PNG): ")
            if encode_message(image_path, message, output_path):
                print(f"Output saved to {output_path}")
            else:
                print("Encoding failed.")
        
        elif choice == '2':
            image_path = input("Enter image path to decode (PNG): ")
            message = decode_message(image_path)
            if message:
                print(f"Decoded message: {message}")
            else:
                print("No message found or error occurred.")
        
        elif choice == '3':
            print("Exiting program.")
            break
        
        else:
            print("Invalid choice. Please try again.")

if _name_ == "_main_":
    main()
