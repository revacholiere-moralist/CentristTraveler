import { Tag } from './tag';

export class Post {
  id: number;
  title: string;
  body: string;
  thumbnail_path: string;
  preview_text: string;
  banner_path: string;
  banner_text: string;
  category_id: number;
  tags: Tag[];
  author_display_name: string;
  author_username: string;
}
